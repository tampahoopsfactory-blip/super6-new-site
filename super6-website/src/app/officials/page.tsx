import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";
import { REGISTER_LINK_PROPS } from "@/lib/links";
import {
  SIGNUP_URL,
  officialsIntro,
  payTiers,
  programFacts,
  programPoints,
  finalCta,
} from "./officials-data";

export const metadata: Metadata = {
  title: "Hiring Officials | Super6 Series LLC Basketball",
  description:
    "Super6 Series LLC is hiring NFHS-certified referees. Weekly weekend games, $20–$27/hr, same-day Sunday payouts. Apply now.",
  alternates: { canonical: "/officials" },
  keywords: [
    "basketball referee jobs Florida",
    "youth basketball official hiring",
    "NFHS referee Florida",
    "Super6 Series LLC referee",
    "weekend basketball official jobs",
    "paid basketball referee Florida",
    "youth sports official hiring",
  ],
  openGraph: {
    title: "Now Hiring Game Officials | Super6 Series LLC",
    description:
      "NFHS-certified referees needed. $20–$27/hr, weekly games, same-day Sunday payouts across Florida and Georgia.",
    url: "/officials",
    type: "website",
    images: ["/media/uploads/officials-refs-shake.jpg"],
  },
};

const jobPostingJsonLd = {
  "@context": "https://schema.org",
  "@type": "JobPosting",
  title: "Basketball Game Official / Referee",
  description:
    "Super6 Series LLC is hiring qualified basketball referees for youth tournament weekends across Florida and Georgia. Bookings are made every Tuesday at 7 PM. Immediate Sunday payouts. Three pay tiers based on experience and consistency.",
  hiringOrganization: {
    "@type": "Organization",
    name: "Super6 Series LLC",
    url: "https://www.thesuper6.com",
  },
  jobLocation: [
    {
      "@type": "Place",
      address: {
        "@type": "PostalAddress",
        addressRegion: "FL",
        addressCountry: "US",
      },
    },
    {
      "@type": "Place",
      address: {
        "@type": "PostalAddress",
        addressRegion: "GA",
        addressCountry: "US",
      },
    },
  ],
  baseSalary: {
    "@type": "MonetaryAmount",
    currency: "USD",
    value: {
      "@type": "QuantitativeValue",
      minValue: 20,
      maxValue: 27,
      unitText: "HOUR",
    },
  },
  employmentType: "PART_TIME",
  workHours: "Weekends only (Saturday and Sunday)",
  scheduleTimezone: "America/New_York",
  applicantLocationRequirements: {
    "@type": "State",
    name: ["Florida", "Georgia"],
  },
  directApply: false,
};

export default function OfficialsPage() {
  return (
    <>
      {/* JSON-LD JobPosting schema */}
      <script
        type="application/ld+json"
        dangerouslySetInnerHTML={{ __html: JSON.stringify(jobPostingJsonLd) }}
      />

      {/* ─── Hero — Editorial "Now Hiring" treatment ─── */}
      <section className="officials-hero">
        <div className="officials-hero-photo">
          <Image
            src="/media/uploads/officials-refs-shake.jpg"
            alt=""
            fill
            priority
            quality={94}
            sizes="(max-width: 968px) 100vw, 55vw"
            aria-hidden="true"
            style={{ objectFit: "cover", objectPosition: "center 35%" }}
          />
        </div>

        <div className="officials-hero-panel">
          <div className="officials-hero-meta">
            <span className="officials-hero-meta-tag">
              {officialsIntro.eyebrow}
            </span>
            <span className="officials-hero-meta-divider" />
            <span>{officialsIntro.meta}</span>
          </div>

          <div className="officials-hero-wordmark" aria-hidden="true">
            <span className="officials-hero-word-primary">
              {officialsIntro.wordmarkPrimary}
            </span>
            <span className="officials-hero-word-accent">
              {officialsIntro.wordmarkAccent}
            </span>
            <span className="officials-hero-word-tag">
              {officialsIntro.wordmarkTag}
            </span>
          </div>

          <h1 className="officials-hero-headline">
            {officialsIntro.headline
              .split("referees.")
              .map((part, i, arr) =>
                i < arr.length - 1 ? (
                  <span key={i}>
                    {part}
                    <em>referees.</em>
                  </span>
                ) : (
                  <span key={i}>{part}</span>
                )
              )}
          </h1>

          <p className="officials-hero-desc">{officialsIntro.desc}</p>

          <div className="officials-hero-cta">
            <a
              href={SIGNUP_URL}
              target="_blank"
              rel="noopener noreferrer"
              className="btn btn-orange officials-hero-cta-button"
            >
              Join the Team
              <svg
                width="18"
                height="18"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
                aria-hidden="true"
                style={{ marginLeft: 10 }}
              >
                <path d="M5 12h14M13 5l7 7-7 7" />
              </svg>
            </a>
            <span className="officials-hero-cta-note">
              Opens our official application form.
            </span>
          </div>
        </div>
      </section>

      {/* ─── Pay structure ─── */}
      <section className="section section-warm officials-pay">
        <div className="container-xl">
          <div className="officials-section-intro">
            <p className="section-label">Pay Structure</p>
            <h2 className="section-heading">
              Three tiers. <em>Built for officials.</em>
            </h2>
            <p className="section-desc">
              Officials are paid by the hour. Tier placement is based on
              experience level and consistency with Super6 Series LLC
              events — show up, call clean ball, and you move up.
            </p>
          </div>

          <div className="officials-pay-grid">
            {payTiers.map((tier) => (
              <article key={tier.label} className="officials-pay-card">
                <div className="officials-pay-rate">
                  <span className="officials-pay-rate-num">{tier.rate}</span>
                  <span className="officials-pay-rate-unit">{tier.unit}</span>
                </div>
                <h3 className="officials-pay-label">{tier.label}</h3>
                <p className="officials-pay-note">{tier.note}</p>
              </article>
            ))}
          </div>

          <div className="officials-pay-facts">
            {programFacts.map((f) => (
              <div key={f.label} className="officials-pay-fact">
                <span className="officials-pay-fact-label">{f.label}</span>
                <span className="officials-pay-fact-value">{f.value}</span>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* ─── Program details ─── */}
      <section className="section officials-program">
        <div className="container-xl">
          <div className="officials-program-grid">
            <aside className="officials-program-side">
              <p className="section-label">Program Details</p>
              <h2
                className="section-heading"
                style={{ fontSize: "clamp(28px, 3.4vw, 44px)" }}
              >
                What you should <em>know.</em>
              </h2>
              <p className="section-desc">
                Read the program guidelines below. When you&rsquo;re ready
                to apply, click the sign-up button — selections lock in
                your city and date availability.
              </p>
            </aside>

            <ul className="officials-program-list">
              {programPoints.map((p, i) => (
                <li key={i} className="officials-program-item">
                  <span className="officials-program-num">
                    {String(i + 1).padStart(2, "0")}
                  </span>
                  <span className="officials-program-text">{p}</span>
                </li>
              ))}
            </ul>
          </div>
        </div>
      </section>

      {/* ─── Final CTA — shared faq-final-cta band ─── */}
      <section className="faq-final-cta">
        <div className="container-xl">
          <p className="faq-final-cta-eyebrow">{finalCta.eyebrow}</p>
          <h2 className="faq-final-cta-title">{finalCta.title}</h2>
          <p className="faq-final-cta-sub">{finalCta.sub}</p>
          <div className="faq-final-cta-actions">
            <a
              href={SIGNUP_URL}
              target="_blank"
              rel="noopener noreferrer"
              className="btn-hero btn-hero-primary"
            >
              Join the Team
            </a>
            <Link {...REGISTER_LINK_PROPS} className="btn-hero btn-hero-secondary">
              Register a team
            </Link>
          </div>
        </div>
      </section>
    </>
  );
}
