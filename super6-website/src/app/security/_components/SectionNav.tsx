"use client";

import SectionIcon from "./SectionIcon";
import type { SecuritySection } from "../security-data";

type Props = {
  sections: SecuritySection[];
  activeId: string | null;
  onJump: (id: string) => void;
  counts?: Record<string, number>;
};

export default function SectionNav({
  sections,
  activeId,
  onJump,
  counts,
}: Props) {
  return (
    <>
      <nav className="faq-rail" aria-label="Security sections">
        <p className="faq-rail-eyebrow">
          <span className="faq-rail-eyebrow-mark" aria-hidden="true" />
          On this page
        </p>
        <ol className="faq-rail-list">
          {sections.map((s) => {
            const count = counts?.[s.id];
            const dim = count === 0;
            const isActive = activeId === s.id;
            return (
              <li key={s.id}>
                <a
                  href={`#${s.id}`}
                  className={`faq-rail-link ${isActive ? "is-active" : ""} ${dim ? "is-dim" : ""}`}
                  onClick={(e) => {
                    e.preventDefault();
                    onJump(s.id);
                  }}
                  aria-current={isActive ? "true" : undefined}
                >
                  <span className="faq-rail-icon">
                    <SectionIcon
                      name={s.icon}
                      size={14}
                      strokeWidth={2}
                    />
                  </span>
                  <span className="faq-rail-num">{s.number}</span>
                  <span className="faq-rail-label">{s.label}</span>
                  {typeof count === "number" && counts ? (
                    <span className="faq-rail-count">{count}</span>
                  ) : null}
                </a>
              </li>
            );
          })}
        </ol>
      </nav>

      <nav className="faq-chipbar-wrap" aria-label="Security sections (mobile)">
        <ul className="faq-chipbar">
          {sections.map((s) => {
            const count = counts?.[s.id];
            const dim = count === 0;
            const isActive = activeId === s.id;
            return (
              <li key={s.id}>
                <a
                  href={`#${s.id}`}
                  className={`faq-chip ${isActive ? "is-active" : ""} ${dim ? "is-dim" : ""}`}
                  onClick={(e) => {
                    e.preventDefault();
                    onJump(s.id);
                  }}
                  aria-current={isActive ? "true" : undefined}
                >
                  <SectionIcon
                    name={s.icon}
                    size={13}
                    strokeWidth={2.2}
                    className="faq-chip-icon"
                  />
                  <span className="faq-chip-num">{s.number}</span>
                  <span className="faq-chip-label">{s.label}</span>
                </a>
              </li>
            );
          })}
        </ul>
      </nav>
    </>
  );
}
