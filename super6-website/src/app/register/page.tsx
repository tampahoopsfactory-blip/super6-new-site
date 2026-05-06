import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";
import { REGISTER_LINK_PROPS } from "@/lib/links";
import { siteSmsHref } from "@/data/site";

/* ─── /register — server component
   Same editorial pattern as /faq and /rules:
   1. Editorial split hero (faq-hero classes)
   2. Single dark "$99" action-bar card (mirrors the homepage HeroActionBar
      treatment so the page reads as one clean, decisive moment instead of
      a wall of text)
   3. Final CTA band (faq-final-cta classes)

   All registration buttons drive to Exposure Events. */

export const metadata: Metadata = {
  title: "Register Your Team",
  description:
    "Register your team for the 2026 Super6 Series LLC season. All registration runs through Exposure Events — our scheduling and tournament management partner. $99 per event or $899 for the full 10-event season.",
  alternates: { canonical: "/register" },
  openGraph: {
    title: "Register Your Team | Super6 Series LLC",
    description:
      "Lock in your spot for the 2026 Super6 Series LLC season — Florida and Georgia, every weekend. $99 single tournament or $899 season pass.",
    url: "/register",
    type: "website",
  },
};

const stats = [
  { label: "Single tournament · $99" },
  { label: "Season pass · $899 / 10 events" },
  { label: "FL + GA · every weekend" },
  { label: "NFHS-certified officials" },
];

export default function RegisterPage() {
  return (
    <>
      {/* Editorial split hero — mirrors /faq and /rules */}
      <section className="faq-hero">
        <div className="faq-hero-photo">
          <Image
            src="/media/curated/register-hero-matchup.png"
            alt="Super6 tournament basketball — drive defended one-on-one on the hardwood."
            fill
            priority
            quality={94}
            sizes="(max-width: 968px) 100vw, 55vw"
            style={{ objectFit: "cover", objectPosition: "center 42%" }}
          />
        </div>

        <div className="faq-hero-panel">
          <div className="faq-hero-meta">
            <span className="faq-hero-meta-tag">REGISTER</span>
          </div>

          <div className="faq-hero-wordmark" aria-hidden="true">
            <span className="faq-hero-word-primary">SECURE</span>
            <span className="faq-hero-word-accent">your spot.</span>
            <span className="faq-hero-word-tag">via Exposure Events</span>
          </div>

          <h1 className="faq-hero-headline">
            Register your <em>team.</em>
          </h1>

          <p className="faq-hero-desc">
            All Super6 Series LLC registration runs through{" "}
            <Link {...REGISTER_LINK_PROPS} className="faq-link faq-link--inverse">
              Exposure Events
            </Link>{" "}
            — our trusted scheduling and tournament management partner. Pick
            an event, lock in your spot, and we&rsquo;ll see you on the floor.{" "}
            <Link href="/rules" className="faq-link faq-link--inverse">
              Read the rule book →
            </Link>
          </p>

          <div className="faq-hero-index">
            <span className="faq-hero-index-num">$99</span>
            <div className="faq-hero-index-text">
              <span className="faq-hero-index-label">Per team / per event</span>
              <span className="faq-hero-index-sub">
                Or lock in all 10 events with the $899 Season Pass — three
                guaranteed games, NFHS-certified officials, championship
                bracket entry.
              </span>
            </div>
          </div>
        </div>
      </section>

      {/* Single dark editorial card — same format as homepage HeroActionBar,
          slightly different copy + stat strip. Replaces the previous
          word-heavy "how it works / primary CTA / pricing" stack. */}
      <section className="hero-action-bar" aria-label="Register on Exposure Events">
        <div className="container-xl hero-action-bar-inner">
          <article className="hero-action-bar-card">
            <div className="hero-action-bar-grid">
              <div className="hero-action-bar-price">
                <p className="hero-action-bar-price-eyebrow">Per team</p>
                <p className="hero-action-bar-price-amount">
                  <span className="hero-action-bar-price-currency">$</span>
                  <span className="hero-action-bar-price-value">99</span>
                </p>
                <p className="hero-action-bar-price-unit">
                  per event · season pass $899
                </p>
              </div>

              <div className="hero-action-bar-rail" aria-hidden="true" />

              <div className="hero-action-bar-copy">
                <p className="hero-action-bar-eyebrow">Exposure Events</p>
                <h2 className="hero-action-bar-heading">
                  Register in <em>minutes.</em>
                </h2>
                <p className="hero-action-bar-sub">
                  Secure checkout, instant confirmation, and the full 2026
                  calendar in one place &mdash; the same platform trusted by
                  300+ clubs across the Southeast.
                </p>
                <div className="hero-action-bar-actions">
                  <Link
                    {...REGISTER_LINK_PROPS}
                    className="btn-hero btn-hero-primary"
                  >
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

      {/* Final CTA — same band as /faq and /rules */}
      <section className="faq-final-cta">
        <div className="container-xl">
          <p className="faq-final-cta-eyebrow">Don&rsquo;t overthink it.</p>
          <h2 className="faq-final-cta-title">
            Bring the team. We&rsquo;ll handle the rest.
          </h2>
          <p className="faq-final-cta-sub">
            Three games guaranteed. Certified officials. A weekend your
            players will remember.
          </p>
          <div className="faq-final-cta-actions">
            <Link
              {...REGISTER_LINK_PROPS}
              className="btn-hero btn-hero-primary"
            >
              Register on Exposure Events
            </Link>
            <a href={siteSmsHref} className="btn-hero btn-hero-secondary">
              Text Super6
            </a>
          </div>
        </div>
      </section>
    </>
  );
}
