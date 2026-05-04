"use client";

import { useEffect, useRef, useState } from "react";
import { Plus } from "lucide-react";
import ReactMarkdown from "react-markdown";
import type { RuleItem as RuleItemType } from "../rules-data";

/* ─── Single accordion card for a rule.
   Mirrors src/app/faq/_components/FAQItem.tsx exactly except for:
   - prop name `body` instead of `a`
   - data-attribute `data-rule-slug` to keep rules and FAQ slugs partitioned

   Uses height-based animation (useState + ref measurement + ResizeObserver),
   not the grid-template-rows trick (Turbopack/Tailwind v4 incompatible).
   Reuses the faq-card-* CSS classes for visual parity. */

type Props = {
  item: RuleItemType;
  open: boolean;
  onToggle: () => void;
  highlight?: string;
  registerRef?: (el: HTMLDivElement | null) => void;
};

function withHighlight(text: string, term: string): React.ReactNode {
  if (!term.trim()) return text;
  try {
    const safeTerm = term.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
    const re = new RegExp(`(${safeTerm})`, "ig");
    const parts = text.split(re);
    return parts.map((part, i) =>
      re.test(part) ? (
        <mark key={i} className="faq-mark">
          {part}
        </mark>
      ) : (
        <span key={i}>{part}</span>
      )
    );
  } catch {
    return text;
  }
}

export default function RulesItem({
  item,
  open,
  onToggle,
  highlight = "",
  registerRef,
}: Props) {
  const buttonId = `rules-q-${item.slug}`;
  const panelId = `rules-a-${item.slug}`;
  const innerRef = useRef<HTMLDivElement>(null);
  const [maxHeight, setMaxHeight] = useState<number | null>(null);

  useEffect(() => {
    if (!innerRef.current) return;
    const measure = () => {
      if (!innerRef.current) return;
      setMaxHeight(innerRef.current.scrollHeight);
    };
    measure();
    if (typeof ResizeObserver === "undefined") return;
    const ro = new ResizeObserver(measure);
    ro.observe(innerRef.current);
    return () => ro.disconnect();
  }, [open, highlight, item.body]);

  return (
    <div
      ref={registerRef}
      id={item.slug}
      className={`faq-card ${open ? "is-open" : ""} ${item.pending ? "is-pending" : ""}`}
      data-rule-slug={item.slug}
    >
      <h3 className="faq-card-heading">
        <button
          id={buttonId}
          type="button"
          className="faq-card-trigger"
          aria-expanded={open}
          aria-controls={panelId}
          onClick={onToggle}
        >
          <span className="faq-card-q-text">
            {withHighlight(item.title, highlight)}
          </span>
          {item.pending ? (
            <span
              className="faq-card-pending"
              aria-label="Pending TK confirmation"
            >
              TODO
            </span>
          ) : null}
          <span className="faq-card-icon" aria-hidden="true">
            <Plus size={16} strokeWidth={2.2} />
          </span>
        </button>
      </h3>

      <div
        id={panelId}
        role="region"
        aria-labelledby={buttonId}
        className="faq-card-panel"
        aria-hidden={open ? undefined : true}
        style={{
          maxHeight: open ? (maxHeight ?? 1200) : 0,
        }}
      >
        <div ref={innerRef} className="faq-card-panel-inner">
          <div className="faq-card-a">
            <ReactMarkdown
              components={{
                a: ({ href, children, ...rest }) => (
                  <a
                    {...rest}
                    href={href}
                    className="faq-link"
                    {...(href?.startsWith("http")
                      ? { target: "_blank", rel: "noopener noreferrer" }
                      : {})}
                  >
                    {typeof children === "string"
                      ? withHighlight(children, highlight)
                      : children}
                  </a>
                ),
                p: ({ children }) => <p>{children}</p>,
                ul: ({ children }) => (
                  <ul className="faq-md-list">{children}</ul>
                ),
                ol: ({ children }) => (
                  <ol className="faq-md-list faq-md-list--ordered">
                    {children}
                  </ol>
                ),
                li: ({ children }) => <li>{children}</li>,
                strong: ({ children }) => <strong>{children}</strong>,
                em: ({ children }) => <em>{children}</em>,
                code: ({ children }) => (
                  <code className="faq-md-code">{children}</code>
                ),
              }}
            >
              {item.body}
            </ReactMarkdown>
          </div>
        </div>
      </div>
    </div>
  );
}
