"use client";

import Link from "next/link";
import { REGISTER_LINK_PROPS } from "@/lib/links";

const stats = [
  { label: "Every weekend" },
  { label: "Florida + Georgia" },
  { label: "NFHS-certified officials" },
  { label: "300+ clubs · 24k athletes annually" },
];

export default function HeroActionBar() {
  return (
    <section className="hero-action-bar" aria-label="The Super6 Standard">
      <div className="container-xl hero-action-bar-inner">
        <article className="hero-action-bar-card">
          {/* Brand reveal — SUPER6 is the largest element, first brand impression */}
          <div className="hero-action-bar-brand">
            <p className="hero-action-bar-brand-above">The</p>
            <h2 className="hero-action-bar-brand-name">
              SUPER<span className="hero-action-bar-brand-6">6</span>
            </h2>
            <p className="hero-action-bar-brand-below">Standard</p>
          </div>

          <div className="hero-action-bar-rail-h" aria-hidden="true" />

          <div className="hero-action-bar-grid">
            <div className="hero-action-bar-price">
              <p className="hero-action-bar-price-eyebrow">Price of entry</p>
              <p className="hero-action-bar-price-amount">
                <span className="hero-action-bar-price-currency">$</span>
                <span className="hero-action-bar-price-value">99</span>
              </p>
              <p className="hero-action-bar-price-unit">per team / per event</p>
            </div>

            <div className="hero-action-bar-rail" aria-hidden="true" />

            <div className="hero-action-bar-copy">
              <h3 className="hero-action-bar-heading">
                Home of the <em>$99 tournament.</em>
              </h3>
              <p className="hero-action-bar-sub">
                Affordable, high-quality youth basketball &mdash; every weekend,
                across Florida and Georgia.
              </p>
              <div className="hero-action-bar-actions">
                <Link {...REGISTER_LINK_PROPS} className="btn-hero btn-hero-primary">
                  Register your team
                </Link>
              </div>
            </div>
          </div>

          <ul className="hero-action-bar-strip" aria-label="What you get">
            {stats.map((s) => (
              <li key={s.label}>{s.label}</li>
            ))}
          </ul>
        </article>
      </div>
    </section>
  );
}
