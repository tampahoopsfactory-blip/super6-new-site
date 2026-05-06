"use client";

import {
  useCallback,
  useEffect,
  useMemo,
  useRef,
  useState,
} from "react";
import FAQSearch from "./FAQSearch";
import SectionNav from "./SectionNav";
import FAQSection from "./FAQSection";
import type { FaqSection } from "../faq-data";

/* ─── Top-level client wrapper for the FAQ.
   Responsibilities:
   - search query state + per-item match index (case-insensitive)
   - single-open accordion across the page (one Q open at a time, per spec)
   - deep-link support: /faq#slug auto-expands the matching item + scrolls
   - scroll-spy: updates the active section id in SectionNav as user scrolls
   - URL hash sync: clicking a Q updates the hash without history bloat
*/

type Props = {
  sections: FaqSection[];
};

const HEADER_OFFSET = 96; // account for sticky site nav when scrolling

function buildSlugSet(sections: FaqSection[]): Set<string> {
  const s = new Set<string>();
  sections.forEach((sec) => sec.items.forEach((i) => s.add(i.slug)));
  return s;
}

function buildSectionForSlug(sections: FaqSection[]): Record<string, string> {
  const map: Record<string, string> = {};
  sections.forEach((sec) => sec.items.forEach((i) => (map[i.slug] = sec.id)));
  return map;
}

export default function FAQClient({ sections }: Props) {
  const [query, setQuery] = useState("");
  const [openSlug, setOpenSlug] = useState<string | null>(null);
  const [activeId, setActiveId] = useState<string | null>(sections[0]?.id ?? null);

  const slugSet = useMemo(() => buildSlugSet(sections), [sections]);
  const sectionForSlug = useMemo(
    () => buildSectionForSlug(sections),
    [sections]
  );

  // Refs for scroll-spy + deep-link scroll
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

  // ─── Filtering: build the visible-slug set + visible-count per section.
  const filterTerm = query.trim().toLowerCase();
  const filterMatches = useMemo(() => {
    if (!filterTerm) return null;
    const set = new Set<string>();
    sections.forEach((sec) => {
      sec.items.forEach((item) => {
        const haystack = (item.q + " " + item.a).toLowerCase();
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

  // ─── Toggle handler: single-open across the page.
  const handleToggle = useCallback(
    (slug: string) => {
      setOpenSlug((cur) => (cur === slug ? null : slug));
      // Update hash without scrolling. We only set the hash on open.
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

  // ─── Smooth-scroll jump from SectionNav clicks.
  const handleJump = useCallback(
    (id: string) => {
      const el = headingRefs.current.get(id);
      if (!el) return;
      const top =
        el.getBoundingClientRect().top + window.scrollY - HEADER_OFFSET;
      window.scrollTo({ top, behavior: "smooth" });
      setActiveId(id);
      // Reflect in URL but don't push history
      const url = new URL(window.location.href);
      url.hash = id;
      window.history.replaceState(null, "", url.toString());
    },
    []
  );

  // ─── Deep-link on mount: if hash matches a slug, expand + scroll. If matches
  //     a section id, just scroll. State updates are deferred via setTimeout
  //     so they happen after paint (avoids React 19 set-state-in-effect rule
  //     and gives the panel a frame to mount before we measure for scroll).
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

      // Section anchor (e.g. #app)
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

  // ─── Scroll-spy: pick the section whose heading is most recently above the
  //     viewport offset. IntersectionObserver with multiple thresholds + a
  //     simple "topmost in view" tie-breaker.
  useEffect(() => {
    if (typeof window === "undefined") return;
    const observer = new IntersectionObserver(
      (entries) => {
        // Find the heading that's currently closest to the top offset.
        const visible = entries
          .filter((e) => e.isIntersecting)
          .map((e) => ({
            id: (e.target as HTMLElement).dataset.sectionId ?? "",
            top: e.boundingClientRect.top,
          }))
          .filter((e) => e.id);
        if (visible.length === 0) return;
        // Closest to (but not above) the offset wins
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
    // Re-run when filter changes — sections may unmount/remount.
  }, [filterMatches]);

  // ─── Empty-state when no matches.
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
            <FAQSearch
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
                Try a different keyword — registration, schedule, gate,
                refunds, uniforms, app — or{" "}
                <a href="/contact" className="faq-link">
                  contact Super6 Series LLC directly
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
              <FAQSection
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
