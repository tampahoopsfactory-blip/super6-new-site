"use client";

import { Search, X } from "lucide-react";

/* ─── Search input for the rules accordion.
   Mirrors src/app/faq/_components/FAQSearch.tsx — controlled component,
   ESC-to-clear, aria-live status. Uses the existing faq-search-* CSS
   classes verbatim to share the editorial style block. */

type Props = {
  value: string;
  onChange: (v: string) => void;
  visibleCount: number;
  totalCount: number;
};

export default function RulesSearch({
  value,
  onChange,
  visibleCount,
  totalCount,
}: Props) {
  const isFiltering = value.trim().length > 0;

  return (
    <div className="faq-search">
      <label htmlFor="rules-search-input" className="faq-search-label">
        <Search size={16} strokeWidth={2.2} aria-hidden="true" />
        <span className="visually-hidden">Search the rules</span>
      </label>
      <input
        id="rules-search-input"
        type="search"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        onKeyDown={(e) => {
          if (e.key === "Escape") onChange("");
        }}
        placeholder="Search uniforms, fouls, eligibility, mercy, forfeit…"
        autoComplete="off"
        spellCheck={false}
        aria-describedby="rules-search-status"
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
        id="rules-search-status"
        className="faq-search-status"
        role="status"
        aria-live="polite"
      >
        {isFiltering
          ? `${visibleCount} of ${totalCount} rules`
          : `${totalCount} rules`}
      </p>
    </div>
  );
}
