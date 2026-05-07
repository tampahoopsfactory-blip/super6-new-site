import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";
import { REGISTER_LINK_PROPS } from "@/lib/links";
import { siteSmsHref } from "@/data/site";

/* ─── /ai-events — AI – Events product showcase
   FROZEN TOP HERO: first <section className="rules-hero ai-events-hero"> — copy,
   two-stack panel (ai-events-hero-panel + stack-top / stack-bottom), hero <Image>
   props (no inline crop). Do NOT change unless TK explicitly asks.
   CSS: frozen banner block in globals.css (.ai-events-hero … through bullet ::before).
   Rules: .cursor/rules/ai-events-frozen-top.mdc · Photo: ai-events-hero-image.mdc

   Below that section: safe to iterate — patron-facing copy only (no SKUs,
   backends, or how-built detail unless TK asks). See ai-events-frozen-top.mdc. */

export const metadata: Metadata = {
  title: "AI – Events",
  description:
    "Super6 Series LLC AI – Events: cashless gate admission, quick check-in, and secure re-entry — so families spend less time in line and more time in the gym.",
  alternates: { canonical: "/ai-events" },
  openGraph: {
    title: "AI – Events | Super6 Series LLC",
    description:
      "Pay from your phone, check in fast, move through the gate with confidence — Super6 Series LLC AI – Events.",
    url: "/ai-events",
    type: "website",
    images: [
      {
        url: "/media/uploads/ai-events-gate-kiosk.png",
        width: 1600,
        height: 900,
        alt: "Super6 Series LLC AI – Events gate kiosk in use during check-in",
      },
    ],
  },
  twitter: {
    card: "summary_large_image",
    title: "AI – Events | Super6 Series LLC",
    description:
      "Cashless gate admission. Quick check-in. Secure re-entry. Less line, more gym time.",
    images: ["/media/uploads/ai-events-gate-kiosk.png"],
  },
};

/* ─── Three-step gate flow — customer-facing copy only (no SKUs, backends,
   APIs, or how-it-was-built). Describe what patrons experience and why it
   matters. ─── */
type Step = {
  number: string;
  eyebrow: string;
  title: string;
  body: string;
  bullets: string[];
  photoSrc: string | null;
  photoAlt: string;
  /** Natural dimensions — when set with `photoSrc`, image is shown in full (no crop) */
  photoWidth?: number;
  photoHeight?: number;
  /** Optional focal point when using `fill` + cover (no intrinsic dimensions) */
  photoObjectPosition?: string;
};

const steps: Step[] = [
  {
    number: "01",
    eyebrow: "Step one · Pay & enroll",
    title: "Register, pay, and capture your face — once.",
    body:
      "At the desk or kiosk you wrap up the boring stuff in one pass: your admission is paid and confirmed, your ticket is tied to you, and a quick face capture saves the profile that every lane will recognize later — no redoing paperwork every time you circle back for re-entry.",
    bullets: [
      "Cashless payment — Cash App, Venmo, Apple Pay, Zelle, or major cards",
      "Registration locks in day or weekend admission before you head to the lane",
      "Face capture enrolls you so your ticket isn’t just a QR — it’s you",
      "Staff stays close if you need help finding your purchase or finishing enrollment",
    ],
    photoSrc: "/media/uploads/ai-events-step-1-face-capture.jpg",
    photoAlt:
      "Guest at a Super6 gate kiosk beside branded signage; gym entrance visible in the background.",
    photoWidth: 1024,
    photoHeight: 999,
  },
  {
    number: "02",
    eyebrow: "Step two · Verify",
    title: "The reader recognizes you — access is triggered.",
    body:
      "When you reach the lane, you’re not presenting a bundle of screenshots. You step to the reader, it compares what it sees now with the enrollment from step one, and a clean match is what tells the system you’re cleared — calm, visible, and built for crowded gyms.",
    bullets: [
      "Face the reader squarely — give it the same unobstructed view you gave at enrollment",
      "Live recognition compares against your saved profile, not a printed workaround",
      "A positive match signals the lane — you’ll see confirmation before anything moves",
      "Built for one patron at a time so nobody slips through on someone else’s admit",
    ],
    photoSrc: "/media/uploads/ai-events-step-2-turnstile.jpg",
    photoAlt:
      "Guest at a Super6 entrance turnstile with on-screen verification; gym floor visible beyond the lane.",
    photoWidth: 1024,
    photoHeight: 957,
  },
  {
    number: "03",
    eyebrow: "Step three · Pass through",
    title: "The turnstile opens — step into the gym.",
    body:
      "Verification has already done its job: the turnstile unlocks for your crossing only, the arms complete their cycle, and you’re through — same rhythm when you duck out for concessions or fresh air and come back while your admission is still good.",
    bullets: [
      "Gate opens only after a verified admit — no squeezing through on momentum alone",
      "One complete rotation per patron keeps tailgating from wrecking the line",
      "Your phone stays handy for staff prompts, but your face already carried the proof",
      "Re-entry repeats the same verify-and-pass cadence for your day or weekend pass",
    ],
    photoSrc: "/media/uploads/ai-events-step-3-enter-lane.jpg",
    photoAlt:
      "Guest passing through a Super6 turnstile with verified face match on screen; basketball court beyond the entrance.",
    photoWidth: 1024,
    photoHeight: 768,
  },
];

/* Benefits grid: short cards, bullets carry the story. See .cursor/rules/super6-copy-no-dashes.mdc */
type Benefit = {
  tag: string;
  title: string;
  bullets: string[];
};

const benefits: Benefit[] = [
  {
    tag: "Weekend pass",
    title: "Enroll once, move all weekend.",
    bullets: [
      "Pay and capture your face the first time through",
      "Friday to Sunday you come and go on the same ticket",
    ],
  },
  {
    tag: "Day pass",
    title: "One day means one day.",
    bullets: [
      "Free in and out until the day ends",
      "Access closes with the day you bought",
    ],
  },
  {
    tag: "Next event",
    title: "Already on file? Just pay.",
    bullets: [
      "Your face profile stays saved between events",
      "Buy a new pass and the reader knows you",
    ],
  },
  {
    tag: "Safer doors",
    title: "Calm lines, clear boundaries.",
    bullets: [
      "Banned guests are stopped before the stands",
      "Honest crowds keep moving without a lecture",
    ],
  },
];

export default function AiEventsPage() {
  return (
    <>
      {/* ═══ FROZEN — TK-approved top hero. Do not edit unless TK explicitly requests.
           Rule: .cursor/rules/ai-events-frozen-top.mdc ═══ */}
      <section className="rules-hero ai-events-hero">
        <div className="rules-hero-photo">
          {/* Hero crop: object-fit/position ONLY in globals.css — see .cursor/rules/ai-events-hero-image.mdc */}
          <Image
            src="/media/uploads/ai-events-gate-kiosk.png"
            alt="Staff assisting a patron at an AI – Events gate kiosk with a facial recognition terminal and tablet check-in."
            fill
            priority
            quality={100}
            sizes="(max-width: 968px) 100vw, 55vw"
          />
        </div>

        <div className="rules-hero-panel ai-events-hero-panel">
          <div className="ai-events-hero-stack-top">
            <div className="rules-hero-meta">
              <span className="rules-hero-meta-tag">AI – EVENTS</span>
              <span className="rules-hero-meta-divider" />
              <span>BIOMETRIC · CASHLESS · TURNSTILE-READY</span>
            </div>

            <div className="rules-hero-wordmark" aria-hidden="true">
              <span className="rules-hero-word-game">AI</span>
              <span className="rules-hero-word-rules">engineering</span>
              <span className="rules-hero-word-book">
                Assuring event safety
              </span>
            </div>

            <h1 className="rules-hero-headline">
              Biometric AI engineering for <em>safer events.</em>
            </h1>
          </div>

          <div className="ai-events-hero-stack-bottom">
            <div className="rules-hero-index">
              <div className="rules-hero-index-text">
                <span className="rules-hero-index-label">
                  AI checkpoint · In & out
                </span>
                <ul
                  className="rules-hero-index-bullets"
                  aria-label="AI biometric checkpoint highlights"
                >
                  <li>
                    <strong>Super6 Series LLC IP (intellectual property).</strong> Proprietary biometric AI designed solution — entry, exit, re-entry.
                  </li>
                  <li>
                    <strong>One of a kind.</strong> Enroll once · verify every crossing.
                  </li>
                  <li>
                    <strong>No tailgating.</strong> Turnstiles — one patron per cycle.
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Process intro (copy rules: .cursor/rules/super6-copy-no-dashes.mdc) */}
      <section className="section section-warm" aria-labelledby="ai-flow-title">
        <div className="container-xl">
          <p className="section-label">How it works</p>
          <h2 id="ai-flow-title" className="section-heading">
            Three steps. <em>You&apos;re in.</em>
          </h2>
          <p className="section-desc">
            Pay at the desk. The reader recognizes you. The turnstile opens.
            Same flow on every return trip during your ticket.
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
                <span className="gate-step-watermark" aria-hidden="true">
                  {step.number}
                </span>
                <div
                  className={`gate-step-media${
                    step.photoSrc &&
                    step.photoWidth != null &&
                    step.photoHeight != null
                      ? " gate-step-media--natural"
                      : ""
                  }`}
                >
                  {step.photoSrc &&
                  step.photoWidth != null &&
                  step.photoHeight != null ? (
                    <Image
                      src={step.photoSrc}
                      alt={step.photoAlt}
                      width={step.photoWidth}
                      height={step.photoHeight}
                      sizes="(max-width: 968px) 100vw, min(720px, 50vw)"
                      quality={100}
                      unoptimized
                      className="gate-step-photo-img gate-step-photo-img--natural"
                    />
                  ) : step.photoSrc ? (
                    <Image
                      src={step.photoSrc}
                      alt={step.photoAlt}
                      fill
                      sizes="(max-width: 968px) 100vw, min(720px, 50vw)"
                      quality={100}
                      unoptimized
                      className="gate-step-photo-img"
                      style={{
                        objectFit: "cover",
                        objectPosition:
                          step.photoObjectPosition ?? "center center",
                      }}
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

      {/* Benefits grid (copy rules: .cursor/rules/super6-copy-no-dashes.mdc) */}
      <section
        className="section section-paper ai-features ai-benefits"
        aria-labelledby="ai-benefits-title"
      >
        <div className="container-xl">
          <p className="section-label">Why it matters</p>
          <h2 id="ai-benefits-title" className="section-heading">
            What you actually <em>get.</em>
          </h2>

          <ul className="ai-features-grid ai-benefits-grid">
            {benefits.map((b) => (
              <li key={b.title} className="ai-feature ai-benefit-card">
                <span className="ai-feature-tag">{b.tag}</span>
                <h3 className="ai-feature-title">{b.title}</h3>
                <ul className="ai-benefit-list" aria-label={`Details: ${b.title}`}>
                  {b.bullets.map((line) => (
                    <li key={line}>{line}</li>
                  ))}
                </ul>
              </li>
            ))}
          </ul>
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
