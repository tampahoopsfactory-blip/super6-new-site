import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";

/* ─── Custom 404 — editorial split hero in the rules-hero pattern.
   Lists the most likely intended pages so a stale URL still lands
   the visitor somewhere useful. Emits a noindex / nofollow header
   so search engines drop any old/typo'd URLs cleanly. */

export const metadata: Metadata = {
  title: "Page not found · Super6 Series LLC",
  description:
    "The page you were looking for doesn't exist. Jump to schedule, locations, FAQ, or contact Super6.",
  robots: { index: false, follow: false },
};

const suggestedLinks = [
  {
    href: "/",
    label: "Home",
    body: "The Super6 home page.",
  },
  {
    href: "/schedule",
    label: "Schedule",
    body: "Tournaments, dates, and divisions.",
  },
  {
    href: "/locations",
    label: "Locations",
    body: "Florida and Georgia venues.",
  },
  {
    href: "/faq",
    label: "FAQ",
    body: "Registration, the app, rules, gate.",
  },
  {
    href: "/rules",
    label: "Rules",
    body: "The full Super6 rule book.",
  },
  {
    href: "/contact",
    label: "Contact",
    body: "Reach Super6 directly — same-day response.",
  },
];

export default function NotFound() {
  return (
    <>
      {/* Hero — editorial split, mirrors /faq, /rules, /coaches */}
      <section className="rules-hero">
        <div className="rules-hero-photo">
          <Image
            src="/media/uploads/coach-huddle.jpg"
            alt=""
            fill
            priority
            quality={92}
            sizes="(max-width: 968px) 100vw, 55vw"
            aria-hidden="true"
            style={{ objectFit: "cover", objectPosition: "center 35%" }}
          />
        </div>

        <div className="rules-hero-panel">
          <div className="rules-hero-meta">
            <span className="rules-hero-meta-tag">404</span>
            <span className="rules-hero-meta-divider" />
            <span>Page Not Found</span>
          </div>

          <div className="rules-hero-wordmark" aria-hidden="true">
            <span className="rules-hero-word-game">OFF</span>
            <span className="rules-hero-word-rules">COURT.</span>
            <span className="rules-hero-word-book">404 · Super6</span>
          </div>

          <h1 className="rules-hero-headline">
            That page is <em>off the schedule.</em>
          </h1>

          <p className="rules-hero-desc">
            The link you followed points at a page that doesn&apos;t exist —
            it may have moved, been renamed, or never shipped. Pick a spot
            below or head back to the{" "}
            <Link href="/" className="faq-link faq-link--inverse">
              homepage
            </Link>
            .
          </p>

          <div className="rules-hero-index">
            <span className="rules-hero-index-num">404</span>
            <div className="rules-hero-index-text">
              <span className="rules-hero-index-label">No content here</span>
              <span className="rules-hero-index-sub">
                Try one of the routes below — they&apos;re the most-visited
                Super6 pages.
              </span>
            </div>
          </div>
        </div>
      </section>

      {/* Suggested links — same card grid pattern as the FAQ section spread */}
      <section className="faq-body" aria-label="Suggested pages">
        <div className="faq-shell">
          <div className="not-found-grid">
            {suggestedLinks.map((link, i) => (
              <Link
                key={link.href}
                href={link.href}
                className="not-found-card"
              >
                <span className="not-found-card-num" aria-hidden="true">
                  {String(i + 1).padStart(2, "0")}
                </span>
                <div>
                  <h2 className="not-found-card-title">{link.label}</h2>
                  <p className="not-found-card-body">{link.body}</p>
                </div>
                <span className="not-found-card-arrow" aria-hidden="true">
                  <svg
                    width="22"
                    height="22"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="1.7"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  >
                    <path d="M5 12h14M13 5l7 7-7 7" />
                  </svg>
                </span>
              </Link>
            ))}
          </div>
        </div>
      </section>

      {/* Final CTA — same band as /faq, /rules, /coaches */}
      <section className="faq-final-cta">
        <div className="container-xl">
          <p className="faq-final-cta-eyebrow">Still lost?</p>
          <h2 className="faq-final-cta-title">
            Tell us what you were trying to find.
          </h2>
          <p className="faq-final-cta-sub">
            Send us a quick note and we&apos;ll point you to the right page.
            Same-day reply during the week.
          </p>
          <div className="faq-final-cta-actions">
            <Link href="/contact" className="btn-hero btn-hero-primary">
              Contact Super6
            </Link>
            <Link href="/" className="btn-hero btn-hero-secondary">
              Back to home
            </Link>
          </div>
        </div>
      </section>
    </>
  );
}
