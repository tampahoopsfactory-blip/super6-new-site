"use client";

import { Search, X } from "lucide-react";

type Props = {
  value: string;
  onChange: (v: string) => void;
  visibleCount: number;
  totalCount: number;
};

export default function SecuritySearch({
  value,
  onChange,
  visibleCount,
  totalCount,
}: Props) {
  const isFiltering = value.trim().length > 0;

  return (
    <div className="faq-search">
      <label htmlFor="security-search-input" className="faq-search-label">
        <Search size={16} strokeWidth={2.2} aria-hidden="true" />
        <span className="visually-hidden">Search the security policy</span>
      </label>
      <input
        id="security-search-input"
        type="search"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        onKeyDown={(e) => {
          if (e.key === "Escape") onChange("");
        }}
        placeholder="Search bag check, gate, weapons, ejection, refunds…"
        autoComplete="off"
        spellCheck={false}
        aria-describedby="security-search-status"
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
        id="security-search-status"
        className="faq-search-status"
        role="status"
        aria-live="polite"
      >
        {isFiltering
          ? `${visibleCount} of ${totalCount} items`
          : `${totalCount} items`}
      </p>
    </div>
  );
}
