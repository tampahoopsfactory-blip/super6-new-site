"use client";

import {
  useCallback,
  useEffect,
  useMemo,
  useRef,
  useState,
} from "react";
import RulesSearch from "./RulesSearch";
import SectionNav from "./SectionNav";
import RulesSection from "./RulesSection";
import type { RuleSection } from "../rules-data";

/* ─── Top-level client wrapper for the rules page.
   Mirrors src/app/faq/_components/FAQClient.tsx — same single-open
   accordion, deep-link expand, scroll-spy via IntersectionObserver,
   URL hash sync, and ESC-to-clear search. The FAQ pattern is reused
   verbatim so the editorial behaviour is identical across content
   pages. */

type Props = {
  sections: RuleSection[];
};

const HEADER_OFFSET = 96;

function buildSlugSet(sections: RuleSection[]): Set<string> {
  const s = new Set<string>();
  sections.forEach((sec) => sec.items.forEach((i) => s.add(i.slug)));
  return s;
}

function buildSectionForSlug(sections: RuleSection[]): Record<string, string> {
  const map: Record<string, string> = {};
  sections.forEach((sec) => sec.items.forEach((i) => (map[i.slug] = sec.id)));
  return map;
}

export default function RulesClient({ sections }: Props) {
  const [query, setQuery] = useState("");
  const [openSlug, setOpenSlug] = useState<string | null>(null);
  const [activeId, setActiveId] = useState<string | null>(
    sections[0]?.id ?? null
  );

  const slugSet = useMemo(() => buildSlugSet(sections), [sections]);
  const sectionForSlug = useMemo(
    () => buildSectionForSlug(sections),
    [sections]
  );

  const headingRefs = useRef<Map<string, HTMLDivElement>>(new Map());
  const itemRefs = useRef<Map<string, HTMLDivElement>>(new Map());

  const registerHeading = useCallback(
    (id: string, el: HTMLDivElement | null) => {
      if (el) headingRefs.current.set(id, el);
      else headingRefs.current.delete(id);
    },
    []
  );
  const registerItem = useCallback(
    (slug: string, el: HTMLDivElement | null) => {
      if (el) itemRefs.current.set(slug, el);
      else itemRefs.current.delete(slug);
    },
    []
  );

  const filterTerm = query.trim().toLowerCase();
  const filterMatches = useMemo(() => {
    if (!filterTerm) return null;
    const set = new Set<string>();
    sections.forEach((sec) => {
      sec.items.forEach((item) => {
        const haystack = (item.title + " " + item.body).toLowerCase();
        if (haystack.includes(filterTerm)) set.add(item.slug);
      });
    });
    return set;
  }, [sections, filterTerm]);

  const sectionCounts = useMemo(() => {
    const map: Record<string, number> = {};
    sections.forEach((sec) => {
      map[sec.id] = sec.items.filter((i) =>
        filterMatches ? filterMatches.has(i.slug) : true
      ).length;
    });
    return map;
  }, [sections, filterMatches]);

  const visibleCount = filterMatches
    ? filterMatches.size
    : sections.reduce((acc, s) => acc + s.items.length, 0);
  const totalCount = sections.reduce((acc, s) => acc + s.items.length, 0);

  const handleToggle = useCallback(
    (slug: string) => {
      setOpenSlug((cur) => (cur === slug ? null : slug));
      if (typeof window !== "undefined") {
        const next = openSlug === slug ? "" : slug;
        const url = new URL(window.location.href);
        if (next) url.hash = next;
        else url.hash = "";
        window.history.replaceState(null, "", url.toString());
      }
    },
    [openSlug]
  );

  const handleJump = useCallback((id: string) => {
    const el = headingRefs.current.get(id);
    if (!el) return;
    const top =
      el.getBoundingClientRect().top + window.scrollY - HEADER_OFFSET;
    window.scrollTo({ top, behavior: "smooth" });
    setActiveId(id);
    const url = new URL(window.location.href);
    url.hash = id;
    window.history.replaceState(null, "", url.toString());
  }, []);

  useEffect(() => {
    if (typeof window === "undefined") return;
    const hash = window.location.hash.replace("#", "");
    if (!hash) return;

    const t = window.setTimeout(() => {
      if (slugSet.has(hash)) {
        setOpenSlug(hash);
        const el = itemRefs.current.get(hash);
        if (el) {
          const top =
            el.getBoundingClientRect().top + window.scrollY - HEADER_OFFSET;
          window.scrollTo({ top, behavior: "smooth" });
        }
        const sectionId = sectionForSlug[hash];
        if (sectionId) setActiveId(sectionId);
        return;
      }

      const headEl = headingRefs.current.get(hash);
      if (headEl) {
        const top =
          headEl.getBoundingClientRect().top + window.scrollY - HEADER_OFFSET;
        window.scrollTo({ top, behavior: "smooth" });
        setActiveId(hash);
      }
    }, 80);

    return () => window.clearTimeout(t);
  }, [slugSet, sectionForSlug]);

  useEffect(() => {
    if (typeof window === "undefined") return;
    const observer = new IntersectionObserver(
      (entries) => {
        const visible = entries
          .filter((e) => e.isIntersecting)
          .map((e) => ({
            id: (e.target as HTMLElement).dataset.sectionId ?? "",
            top: e.boundingClientRect.top,
          }))
          .filter((e) => e.id);
        if (visible.length === 0) return;
        visible.sort((a, b) => Math.abs(a.top) - Math.abs(b.top));
        setActiveId(visible[0].id);
      },
      {
        rootMargin: `-${HEADER_OFFSET}px 0px -55% 0px`,
        threshold: [0, 0.25, 0.5, 1],
      }
    );

    headingRefs.current.forEach((el, id) => {
      el.dataset.sectionId = id;
      observer.observe(el);
    });

    return () => observer.disconnect();
  }, [filterMatches]);

  const hasResults = visibleCount > 0;

  return (
    <div className="faq-shell">
      <div className="faq-shell-grid">
        <aside className="faq-shell-aside">
          <SectionNav
            sections={sections}
            activeId={activeId}
            onJump={handleJump}
            counts={filterMatches ? sectionCounts : undefined}
          />
        </aside>

        <div className="faq-shell-main">
          <div className="faq-search-wrap">
            <RulesSearch
              value={query}
              onChange={setQuery}
              visibleCount={visibleCount}
              totalCount={totalCount}
            />
          </div>

          {!hasResults ? (
            <div className="faq-empty" role="status">
              <p className="faq-empty-eyebrow">No matches</p>
              <h2 className="faq-empty-title">
                Nothing matches &ldquo;{query}&rdquo;.
              </h2>
              <p className="faq-empty-body">
                Try a different keyword — uniforms, fouls, eligibility, mercy,
                forfeit, overtime — or{" "}
                <a href="/contact" className="faq-link">
                  contact Super6 directly
                </a>
                . We respond same-day during the week.
              </p>
              <button
                type="button"
                className="faq-empty-reset"
                onClick={() => setQuery("")}
              >
                Clear search
              </button>
            </div>
          ) : (
            sections.map((sec) => (
              <RulesSection
                key={sec.id}
                section={sec}
                openSlug={openSlug}
                onToggle={handleToggle}
                highlight={query}
                registerHeading={registerHeading}
                registerItem={registerItem}
                visibleSlugs={filterMatches ?? undefined}
              />
            ))
          )}
        </div>
      </div>
    </div>
  );
}
