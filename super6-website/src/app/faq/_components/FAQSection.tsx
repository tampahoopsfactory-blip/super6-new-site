"use client";

import { useCallback } from "react";
import FAQItem from "./FAQItem";
import SectionIcon from "./SectionIcon";
import type { FaqSection } from "../faq-data";

/* ─── Section block — editorial spread with icon medallion + watermark numeral.
   Owns the `openSlug` for its section so only ONE item is open at a time. */

type Props = {
  section: FaqSection;
  openSlug: string | null;
  onToggle: (slug: string) => void;
  highlight?: string;
  registerHeading?: (id: string, el: HTMLDivElement | null) => void;
  registerItem?: (slug: string, el: HTMLDivElement | null) => void;
  hidden?: boolean;
  /** Filtered list of slugs to render. If undefined, render all items. */
  visibleSlugs?: Set<string>;
};

export default function FAQSection({
  section,
  openSlug,
  onToggle,
  highlight,
  registerHeading,
  registerItem,
  hidden,
  visibleSlugs,
}: Props) {
  const setHeadingRef = useCallback(
    (el: HTMLDivElement | null) => {
      registerHeading?.(section.id, el);
    },
    [registerHeading, section.id]
  );

  if (hidden) return null;

  const items = visibleSlugs
    ? section.items.filter((i) => visibleSlugs.has(i.slug))
    : section.items;

  if (items.length === 0) return null;

  return (
    <section
      id={section.id}
      className="faq-section"
      aria-labelledby={`faq-section-${section.id}-title`}
    >
      <header className="faq-section-header" ref={setHeadingRef}>
        {/* Watermark numeral — large, low-contrast, sits behind the title */}
        <span className="faq-section-watermark" aria-hidden="true">
          {section.number}
        </span>

        <div className="faq-section-icon">
          <SectionIcon name={section.icon} size={22} strokeWidth={1.8} />
        </div>

        <div className="faq-section-titlewrap">
          <p className="faq-section-eyebrow">
            <span className="faq-section-eyebrow-rule" aria-hidden="true" />
            Section {section.number}
          </p>
          <h2
            id={`faq-section-${section.id}-title`}
            className="faq-section-title"
          >
            {section.title}
          </h2>
          {section.description ? (
            <p className="faq-section-desc">{section.description}</p>
          ) : null}
          <p className="faq-section-count" aria-hidden="true">
            <span>{items.length}</span> question{items.length !== 1 ? "s" : ""}
          </p>
        </div>
      </header>

      <div className="faq-cards" role="list">
        {items.map((item) => (
          <div role="listitem" key={item.slug}>
            <FAQItem
              item={item}
              open={openSlug === item.slug}
              onToggle={() => onToggle(item.slug)}
              highlight={highlight}
              registerRef={(el) => registerItem?.(item.slug, el)}
            />
          </div>
        ))}
      </div>
    </section>
  );
}
