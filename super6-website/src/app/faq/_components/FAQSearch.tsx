"use client";

import { Search, X } from "lucide-react";

/* ─── Search input.
   Controlled component — parent manages query state and filtering.
   Accessible: real <label>, clear button, ESC to clear. */

type Props = {
  value: string;
  onChange: (v: string) => void;
  /** Total visible questions after filter, for the live status. */
  visibleCount: number;
  /** Total questions on the page. */
  totalCount: number;
};

export default function FAQSearch({
  value,
  onChange,
  visibleCount,
  totalCount,
}: Props) {
  const isFiltering = value.trim().length > 0;

  return (
    <div className="faq-search">
      <label htmlFor="faq-search-input" className="faq-search-label">
        <Search size={16} strokeWidth={2.2} aria-hidden="true" />
        <span className="visually-hidden">Search the FAQ</span>
      </label>
      <input
        id="faq-search-input"
        type="search"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        onKeyDown={(e) => {
          if (e.key === "Escape") onChange("");
        }}
        placeholder="Search registration, schedule, gate, refunds…"
        autoComplete="off"
        spellCheck={false}
        aria-describedby="faq-search-status"
        className="faq-search-input"
      />
      {isFiltering ? (
        <button
          type="button"
          className="faq-search-clear"
          onClick={() => onChange("")}
          aria-label="Clear search"
        >
          <X size={14} strokeWidth={2.2} />
        </button>
      ) : null}
      <p
        id="faq-search-status"
        className="faq-search-status"
        role="status"
        aria-live="polite"
      >
        {isFiltering
          ? `${visibleCount} of ${totalCount} questions`
          : `${totalCount} questions`}
      </p>
    </div>
  );
}
