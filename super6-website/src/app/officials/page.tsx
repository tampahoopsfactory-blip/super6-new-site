import type { Metadata } from "next";
import Image from "next/image";

export const metadata: Metadata = {
  title: "Hiring Officials | Super6 Basketball",
  description:
    "Super 6 is hiring NFHS-certified referees. Weekly weekend games, $20–$27/hr, same-day Sunday payouts. Apply now.",
};

const SIGNUP_URL =
  "https://docs.google.com/forms/d/e/1FAIpQLSd3r9iCxYdoHDCLSzLQpb2I4xu6ukgXIDaIV7tfQiLqsABSag/viewform?usp=header";

const programPoints = [
  "Bookings are made every Tuesday at 7:00 PM.",
  "We are looking for qualified referees who can cover all of our games.",
  "Special consideration is given for long-term consistency in availability and quality of work.",
  "Referee consistency with Super 6 will secure court(s) for games long-term.",
  "If you are only looking for a few games from time-to-time, please still apply.",
  "Select only the city(s) and dates you are available.",
  "Must be available to work the entire event unless prior arrangements are established with Super 6.",
];

const payTiers = [
  {
    rate: "$20",
    unit: "/hr",
    label: "Non-Experienced Officials",
    note: "Building hours under our crew.",
  },
  {
    rate: "$25",
    unit: "/hr",
    label: "Non-Permanent Officials",
    note: "Game-by-game, available as needed.",
  },
  {
    rate: "$27",
    unit: "/hr",
    label: "Permanent Officials",
    note: "Long-term, consistent availability.",
  },
];

const programFacts = [
  { label: "Schedule", value: "Weekends only · Sat & Sun" },
  { label: "Booking Day", value: "Tuesdays at 7 PM" },
  { label: "Coverage", value: "Multi-city tournaments" },
  { label: "Payout", value: "Same-day Sunday" },
];

export default function OfficialsPage() {
  return (
    <>
      {/* Hero — Editorial "Now Hiring" treatment */}
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
            <span className="officials-hero-meta-tag">Now Hiring</span>
            <span className="officials-hero-meta-divider" />
            <span>$20–$27 PER HOUR · WEEKLY GAMES</span>
          </div>

          <div className="officials-hero-wordmark" aria-hidden="true">
            <span className="officials-hero-word-primary">NOW</span>
            <span className="officials-hero-word-accent">HIRING.</span>
            <span className="officials-hero-word-tag">Game Officials</span>
          </div>

          <h1 className="officials-hero-headline">
            We&rsquo;re growing — and looking to expand our great team of{" "}
            <em>referees.</em>
          </h1>

          <p className="officials-hero-desc">
            Apply once and we&rsquo;ll book you weekly. Tuesday 7&nbsp;PM
            assignments, immediate Sunday payouts, and a long-term path for
            officials who show up consistently and call clean ball.
          </p>

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

      {/* Pay structure */}
      <section className="section section-warm officials-pay">
        <div className="container-xl">
          <div className="officials-section-intro">
            <p className="section-label">Pay Structure</p>
            <h2 className="section-heading">
              Three tiers. <em>Built for officials.</em>
            </h2>
            <p className="section-desc">
              Officials are paid by the hour. Tier placement is based on
              experience level and consistency with Super 6 events — show up,
              call clean ball, and you move up.
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

      {/* Program details */}
      <section className="section officials-program">
        <div className="container-xl">
          <div className="officials-program-grid">
            <aside className="officials-program-side">
              <p className="section-label">Program Details</p>
              <h2 className="section-heading" style={{ fontSize: "clamp(28px, 3.4vw, 44px)" }}>
                What you should <em>know.</em>
              </h2>
              <p className="section-desc">
                Read the program guidelines below. When you&rsquo;re ready to
                apply, click the sign-up button — selections lock in your
                city and date availability.
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

      {/* Final CTA */}
      <section className="officials-final-cta">
        <div className="container-xl">
          <div className="officials-final-inner">
            <p className="officials-final-eyebrow">Ready to officiate?</p>
            <h2 className="officials-final-heading">
              Submit your availability and join the rotation.
            </h2>
            <p className="officials-final-desc">
              Bookings go out every Tuesday at 7&nbsp;PM. Apply once — your
              selected cities and dates are how we assign games.
            </p>
            <a
              href={SIGNUP_URL}
              target="_blank"
              rel="noopener noreferrer"
              className="btn btn-orange officials-final-button"
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
          </div>
        </div>
      </section>
    </>
  );
}
