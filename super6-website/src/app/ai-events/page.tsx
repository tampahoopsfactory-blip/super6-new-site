import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";
import { REGISTER_LINK_PROPS } from "@/lib/links";
import { siteSmsHref } from "@/data/site";

/* ─── /ai-events — AI – Events product showcase
   Single page that introduces the gate access platform branded as
   "AI – Events". Hero reuses the editorial split treatment from
   /rules and /coaches (.rules-hero* classes). Body is a scaffold
   covering the features and functionality of the platform; copy
   inside each block is a placeholder TK will direct after first
   review. Slots that need real photos use plain divs with the
   .gate-step-placeholder treatment so a missing asset can never
   break the build. */

export const metadata: Metadata = {
  title: "AI – Events",
  description:
    "AI – Events: Super6 Series LLC's AI-powered gate access platform. Cashless payments, biometric check-in at terminal or turnstile, watchlist screening, and full operator visibility — built for live events.",
  alternates: { canonical: "/ai-events" },
  openGraph: {
    title: "AI – Events | Super6 Series LLC",
    description:
      "AI-powered gate access — pay, capture, verify. Built for events. Powered by Super6 Series LLC.",
    url: "/ai-events",
    type: "website",
  },
};

/* ─── Three-step gate flow (placeholder copy — TK to direct) ─── */
type Step = {
  number: string;
  eyebrow: string;
  title: string;
  body: string;
  bullets: string[];
  photoSrc: string | null;
  photoAlt: string;
};

const steps: Step[] = [
  {
    number: "01",
    eyebrow: "Register · Pay",
    title: "Pay at the gate.",
    body:
      "Patrons step up to the on-site kiosk and pay venue admission with whatever's already on their phone. No cash. The transaction clears server-side via webhook before a ticket is issued.",
    bullets: [
      "Cash App, Venmo, Apple Pay, Zelle, or any major credit card",
      "Daily ticket valid until 23:59:59 same day",
      "Weekend ticket valid through Sunday 23:59:59",
      "Server-side payment verification — no screenshots accepted",
    ],
    photoSrc: null,
    photoAlt: "Patron paying admission at the AI – Events gate kiosk.",
  },
  {
    number: "02",
    eyebrow: "Capture · Terminal",
    title: "Phone + face, captured at the terminal.",
    body:
      "Once payment clears, the kiosk captures the patron's 10-digit phone number plus a 4-digit extension and takes a face capture from the DS-F881 terminal camera. The biometric is bound to the ticket in seconds.",
    bullets: [
      "10-digit phone number + 4-digit extension as the unique identifier",
      "Face captured by the DS-F881 terminal camera",
      "Re-issue tickets later by searching the phone number",
      "Watchlist check runs in the background — supervisor-resolved",
    ],
    photoSrc: null,
    photoAlt:
      "DS-F881 terminal capturing a patron's face during gate registration.",
  },
  {
    number: "03",
    eyebrow: "Verify · Turnstile",
    title: "Walk through the turnstile.",
    body:
      "At the lane, the patron looks at the gate-mounted facial recognition device. The system matches the live face, validates the QR ticket, and releases the DSN-50P turnstile relay — all under a second.",
    bullets: [
      "QR code + biometric match required to unlock the lane",
      "Two capture points — terminal or device — chosen per venue",
      "DSN-50P turnstile relay opens on a successful match",
      "Devices stay time-synced ±2 seconds; offline events queue and sync on reconnect",
    ],
    photoSrc: null,
    photoAlt:
      "Patron passing through an AI – Events turnstile after a successful facial match.",
  },
];

/* ─── Features & functionality grid (placeholder copy — TK to direct).
   Each card represents a capability of the AI – Events platform. Keep
   tags short and punchy; replace bodies with TK-confirmed copy after
   first review. Add or remove cards freely — the grid auto-flows. */
type Feature = {
  tag: string;
  title: string;
  body: string;
};

const features: Feature[] = [
  {
    tag: "Payments",
    title: "Cashless gate, end-to-end.",
    body:
      "Cash App, Venmo, Apple Pay, Zelle, and every major credit card. Webhook-verified before a ticket is ever issued.",
  },
  {
    tag: "Biometrics",
    title: "Face capture at terminal or turnstile.",
    body:
      "Operators choose where the face is captured per venue. Same encrypted template, same identity, validated by the same backend.",
  },
  {
    tag: "Tickets",
    title: "Daily and Weekend ticket types.",
    body:
      "Daily passes expire at 23:59:59 same day. Weekend passes run through Sunday 23:59:59. Devices enforce expiry locally even when offline.",
  },
  {
    tag: "Watchlist",
    title: "Screening with supervisor override.",
    body:
      "Configurable watchlists run in the background on every registration. Supervisors decide deny / allow on a flagged hit. Twilio SMS/MMS notifies security in real time.",
  },
  {
    tag: "Devices",
    title: "DS-F881 terminal · DSN-50P turnstile.",
    body:
      "Time-synced to ±2 seconds, push or polling work mode per deployment, automatic biometric purge on ticket expiry.",
  },
  {
    tag: "Reliability",
    title: "Offline-tolerant, online-synced.",
    body:
      "Devices enforce ticket expiry locally when the network drops; events queue and sync the moment connectivity returns.",
  },
  {
    tag: "Operations",
    title: "Roles, logs, exports.",
    body:
      "Gate Staff, Supervisors, and Administrators get scoped access. Logs, last-sync times, and device health export to CSV on demand.",
  },
  {
    tag: "Branding",
    title: "White-label every venue.",
    body:
      "Logo, name, tagline, colors, fonts. The kiosk wears the venue's brand without custom code.",
  },
  {
    tag: "Integrations",
    title: "Webhooks · Watchlist API · Twilio.",
    body:
      "Plug AI – Events into existing payment, screening, and notification stacks via configurable webhooks and APIs.",
  },
];

export default function AiEventsPage() {
  return (
    <>
      {/* ── Hero — same editorial split as /rules and /coaches ───── */}
      <section className="rules-hero ai-events-hero">
        <div className="rules-hero-photo">
          <Image
            src="/media/uploads/ai-events-gate-kiosk.png"
            alt="Staff assisting a patron at an AI – Events gate kiosk with a facial recognition terminal and tablet check-in."
            fill
            priority
            quality={94}
            sizes="(max-width: 968px) 100vw, 55vw"
            style={{ objectFit: "contain", objectPosition: "center center" }}
          />
        </div>

        <div className="rules-hero-panel">
          <div className="rules-hero-meta">
            <span className="rules-hero-meta-tag">AI – EVENTS</span>
            <span className="rules-hero-meta-divider" />
            <span>BIOMETRIC · CASHLESS · TURNSTILE-READY</span>
          </div>

          <div className="rules-hero-wordmark" aria-hidden="true">
            <span className="rules-hero-word-game">AI</span>
            <span className="rules-hero-word-rules">events</span>
            <span className="rules-hero-word-book">
              Assuring event safety
            </span>
          </div>

          <h1 className="rules-hero-headline">
            Walk up. <em>Get scanned.</em> Walk in.
          </h1>

          <p className="rules-hero-desc">
            AI – Events is the gate access platform behind every Super6 Series LLC
            weekend — cashless payments, biometric check-in at the terminal
            or the turnstile, and a backend that verifies every payment and
            every face before a lane unlocks.{" "}
            <Link href="/rules#admission" className="faq-link faq-link--inverse">
              Read the gate policy →
            </Link>
          </p>

          <div className="rules-hero-index">
            <span className="rules-hero-index-num">03</span>
            <div className="rules-hero-index-text">
              <span className="rules-hero-index-label">Steps in the gate flow</span>
              <span className="rules-hero-index-sub">
                Register & Pay · Capture at Terminal · Verify at Turnstile —
                facial recognition handled at either capture point per venue.
              </span>
            </div>
          </div>
        </div>
      </section>

      {/* ── Process intro band ───────────────────────────────────── */}
      <section className="section section-warm" aria-labelledby="ai-flow-title">
        <div className="container-xl">
          <p className="section-label">The Gate Flow</p>
          <h2 id="ai-flow-title" className="section-heading">
            From <em>parking lot</em> to <em>courtside</em> in three steps.
          </h2>
          <p className="section-desc">
            Built on the DS-F881 terminal and DSN-50P turnstile relay, with a
            cloud backend that verifies every payment before a ticket is
            issued and every face before a lane unlocks.
          </p>
        </div>
      </section>

      {/* ── Three-step showcase ──────────────────────────────────── */}
      <section className="section gate-steps" aria-label="Gate access process">
        <div className="container-xl">
          <ol className="gate-step-list">
            {steps.map((step, idx) => (
              <li
                key={step.number}
                className={`gate-step ${idx % 2 === 1 ? "gate-step--reverse" : ""}`}
              >
                <div className="gate-step-media">
                  {step.photoSrc ? (
                    <Image
                      src={step.photoSrc}
                      alt={step.photoAlt}
                      fill
                      sizes="(max-width: 968px) 100vw, 50vw"
                      style={{ objectFit: "cover" }}
                    />
                  ) : (
                    <div className="gate-step-placeholder" aria-hidden="true">
                      <span className="gate-step-placeholder-num">
                        {step.number}
                      </span>
                      <span className="gate-step-placeholder-label">
                        Photo coming soon
                      </span>
                      <span className="gate-step-placeholder-caption">
                        {step.photoAlt}
                      </span>
                    </div>
                  )}
                </div>

                <div className="gate-step-body">
                  <span className="gate-step-num" aria-hidden="true">
                    {step.number}
                  </span>
                  <p className="gate-step-eyebrow">{step.eyebrow}</p>
                  <h3 className="gate-step-title">{step.title}</h3>
                  <p className="gate-step-desc">{step.body}</p>
                  <ul className="gate-step-bullets">
                    {step.bullets.map((bullet) => (
                      <li key={bullet}>{bullet}</li>
                    ))}
                  </ul>
                </div>
              </li>
            ))}
          </ol>
        </div>
      </section>

      {/* ── Features & Functionality grid ────────────────────────── */}
      <section
        className="section section-paper ai-features"
        aria-labelledby="ai-features-title"
      >
        <div className="container-xl">
          <p className="section-label">Features &amp; Functionality</p>
          <h2 id="ai-features-title" className="section-heading">
            Everything the platform <em>actually does.</em>
          </h2>
          <p className="section-desc">
            A scaffolded list of capabilities — TK will edit each card to
            match the AI – Events go-to-market story. Add, remove, or
            reorder freely.
          </p>

          <ul className="ai-features-grid">
            {features.map((f) => (
              <li key={f.title} className="ai-feature">
                <span className="ai-feature-tag">{f.tag}</span>
                <h3 className="ai-feature-title">{f.title}</h3>
                <p className="ai-feature-body">{f.body}</p>
              </li>
            ))}
          </ul>
        </div>
      </section>

      {/* ── Tech callout — two capture points ────────────────────── */}
      <section className="section" aria-labelledby="ai-tech-title">
        <div className="container-xl">
          <div className="gate-tech-grid">
            <div className="gate-tech-copy">
              <p className="section-label">Where the face is captured</p>
              <h2 id="ai-tech-title" className="section-heading">
                Two capture points. <em>One identity.</em>
              </h2>
              <p className="section-desc">
                The face can be captured at the registration terminal during
                payment, or at the facial recognition device mounted on the
                turnstile itself. Either path produces the same encrypted
                template, bound to the same phone number, validated by the
                same backend. Operators choose per venue based on layout and
                throughput.
              </p>
            </div>
            <div className="gate-tech-points">
              <div className="gate-tech-point">
                <span className="gate-tech-point-tag">Option A</span>
                <h3 className="gate-tech-point-title">DS-F881 Terminal</h3>
                <p className="gate-tech-point-body">
                  Capture the face at the kiosk during registration. Best for
                  venues with a single staffed entry where lines move under
                  staff supervision.
                </p>
              </div>
              <div className="gate-tech-point">
                <span className="gate-tech-point-tag">Option B</span>
                <h3 className="gate-tech-point-title">Turnstile Device</h3>
                <p className="gate-tech-point-body">
                  Capture the face at the gate-mounted device on the way
                  through. Best for high-throughput venues running multiple
                  lanes off a single registration desk.
                </p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* ── Final CTA ────────────────────────────────────────────── */}
      <section className="faq-final-cta">
        <div className="container-xl">
          <p className="faq-final-cta-eyebrow">See it in action.</p>
          <h2 className="faq-final-cta-title">Bring the team.</h2>
          <p className="faq-final-cta-sub">
            Show up to a Super6 Series LLC weekend and experience AI – Events at the
            gate yourself — or lock in your spot now.
          </p>
          <div className="faq-final-cta-actions">
            <Link {...REGISTER_LINK_PROPS} className="btn-hero btn-hero-primary">
              Register your team
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
