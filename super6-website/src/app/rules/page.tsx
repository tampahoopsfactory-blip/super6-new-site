import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";
import RulesClient from "./_components/RulesClient";
import { rulesIntro, rulesSections, totalRules } from "./rules-data";
import { REGISTER_LINK_PROPS } from "@/lib/links";
import { siteSmsHref } from "@/data/site";

/* ─── /rules — server component
   - Editorial split hero (kept from previous design — already matches the
     FAQ pattern visually).
   - No-cash gate policy + payment-methods callout rendered as a bespoke
     section between the hero and the accordion. The rich SVG payment marks
     don't fit the markdown-only RuleItem shape and they earn their own band
     anyway — admission is the highest-traffic question after registration.
   - Two-column shell (sticky rail + scroll-spy + single-open accordion +
     deep-link expand) handled by RulesClient — adapted from FAQClient.

   Audit reference: P1-02 (apply FAQ editorial pattern to /rules). */

export const metadata: Metadata = {
  title: "Tournament Rules",
  description:
    "Official Super6 Series LLC youth basketball tournament rules. Conduct, uniform compliance, game format, player eligibility, tiebreakers, scheduling. NFHS standards.",
  alternates: { canonical: "/rules" },
  openGraph: {
    title: "Tournament Rules | Super6 Series LLC",
    description:
      "Conduct, uniforms, game format, eligibility, tiebreakers, scheduling — the 2026 Super6 Series LLC rule book.",
    url: "/rules",
    type: "website",
  },
};

export default function RulesPage() {
  return (
    <>
      {/* Hero — Editorial Rule Book treatment */}
      <section className="rules-hero">
        <div className="rules-hero-photo">
          <Image
            src="/media/uploads/refs-crew.jpg"
            alt=""
            fill
            priority
            quality={94}
            sizes="(max-width: 968px) 100vw, 55vw"
            aria-hidden="true"
            style={{ objectFit: "cover", objectPosition: "center 30%" }}
          />
        </div>

        <div className="rules-hero-panel">
          <div className="rules-hero-meta">
            <span className="rules-hero-meta-tag">{rulesIntro.eyebrow.toUpperCase()}</span>
            <span className="rules-hero-meta-divider" />
            <span>2026 SEASON · NFHS STANDARD</span>
          </div>

          <div className="rules-hero-wordmark" aria-hidden="true">
            <span className="rules-hero-word-game">GAME</span>
            <span className="rules-hero-word-rules">
              RULES
              <em className="rules-hero-amp">&</em>
            </span>
            <span className="rules-hero-word-book">RULE BOOK</span>
          </div>

          <h1 className="rules-hero-headline">
            Played by the <em>book.</em>
          </h1>

          <p className="rules-hero-desc">
            {rulesIntro.body}{" "}
            <Link href="/faq" className="faq-link faq-link--inverse">
              Open the FAQ →
            </Link>
          </p>

          <div className="rules-hero-index">
            <span className="rules-hero-index-num">
              {String(totalRules).padStart(2, "0")}
            </span>
            <div className="rules-hero-index-text">
              <span className="rules-hero-index-label">Rules in {rulesSections.length} sections</span>
              <span className="rules-hero-index-sub">
                Conduct · Uniforms · Team requirements · Game format ·
                Tiebreakers · Eligibility · Scheduling
              </span>
            </div>
          </div>
        </div>
      </section>

      {/* No-cash gate policy + payment methods — bespoke band, kept from
          the previous /rules design because the SVG payment marks don't
          fit the markdown-only RuleItem shape. */}
      <section
        id="gate-payment"
        className="section section-warm"
        aria-labelledby="gate-payment-title"
      >
        <div className="container-xl">
          <div className="rules-payment-notice" role="note">
            <div className="rules-payment-header">
              <span className="rules-payment-tag">Important</span>
              <span className="rules-payment-cashout" aria-hidden="true">
                <span className="rules-payment-cash-word">CASH</span>
              </span>
            </div>
            <h2 id="gate-payment-title" className="rules-payment-headline">
              We do <em>not</em> accept cash at the gate.
            </h2>
            <p className="rules-payment-sub">
              Pay quickly and securely with any of the following — please
              have your method ready before reaching the front.
            </p>
            <ul className="rules-payment-methods">
              <li className="rules-payment-method">
                <span className="rules-payment-logo" aria-hidden="true">
                  <svg viewBox="0 0 64 64" width="56" height="56">
                    <rect width="64" height="64" rx="14" fill="#FBF7EE" />
                    <text
                      x="32"
                      y="46"
                      textAnchor="middle"
                      fontFamily="var(--font-display), serif"
                      fontSize="44"
                      fontWeight="700"
                      fill="#F35422"
                    >
                      $
                    </text>
                  </svg>
                </span>
                <span className="rules-payment-method-name">Cash App</span>
              </li>

              <li className="rules-payment-method">
                <span className="rules-payment-logo" aria-hidden="true">
                  <svg viewBox="0 0 64 64" width="56" height="56">
                    <rect width="64" height="64" rx="14" fill="#FBF7EE" />
                    <text
                      x="32"
                      y="48"
                      textAnchor="middle"
                      fontFamily="var(--font-display), serif"
                      fontSize="42"
                      fontStyle="italic"
                      fontWeight="700"
                      fill="#F35422"
                    >
                      v
                    </text>
                  </svg>
                </span>
                <span className="rules-payment-method-name">Venmo</span>
              </li>

              <li className="rules-payment-method">
                <span className="rules-payment-logo" aria-hidden="true">
                  <svg viewBox="0 0 64 64" width="56" height="56">
                    <rect width="64" height="64" rx="14" fill="#FBF7EE" />
                    <path
                      d="M33.4 22.4c-1 1.2-2.7 2.2-4.4 2-.2-1.7.6-3.6 1.6-4.7 1.1-1.3 2.9-2.2 4.3-2.3.2 1.8-.5 3.7-1.5 5zm1.5 2.4c-2.5-.1-4.6 1.4-5.8 1.4-1.2 0-3-1.3-5-1.3-2.5 0-4.9 1.5-6.2 3.8-2.7 4.6-.7 11.4 1.9 15.2 1.3 1.9 2.8 4 4.8 3.9 1.9-.1 2.6-1.2 4.9-1.2 2.3 0 3 1.2 5 1.2 2.1 0 3.4-1.9 4.7-3.8 1.5-2.2 2-4.3 2.1-4.4-.1 0-4-1.5-4-6 0-3.7 3.1-5.5 3.2-5.6-1.7-2.5-4.5-2.8-5.5-2.9-2.5-.3-4.6 1.4-5.8 1.4-1.1 0-2.7-1-4.5-1z"
                      fill="#F35422"
                    />
                  </svg>
                </span>
                <span className="rules-payment-method-name">Apple Pay</span>
              </li>

              <li className="rules-payment-method">
                <span className="rules-payment-logo" aria-hidden="true">
                  <svg viewBox="0 0 64 64" width="56" height="56">
                    <rect width="64" height="64" rx="14" fill="#FBF7EE" />
                    <text
                      x="32"
                      y="46"
                      textAnchor="middle"
                      fontFamily="var(--font-display), serif"
                      fontSize="42"
                      fontWeight="700"
                      fill="#F35422"
                    >
                      Z
                    </text>
                  </svg>
                </span>
                <span className="rules-payment-method-name">Zelle</span>
              </li>

              <li className="rules-payment-method">
                <span className="rules-payment-logo" aria-hidden="true">
                  <svg viewBox="0 0 64 64" width="56" height="56">
                    <rect width="64" height="64" rx="14" fill="#FBF7EE" />
                    <rect
                      x="14"
                      y="20"
                      width="36"
                      height="24"
                      rx="3"
                      fill="none"
                      stroke="#F35422"
                      strokeWidth="2.5"
                    />
                    <line
                      x1="14"
                      y1="27"
                      x2="50"
                      y2="27"
                      stroke="#F35422"
                      strokeWidth="2.5"
                    />
                    <rect
                      x="20"
                      y="36"
                      width="9"
                      height="3"
                      fill="#F35422"
                    />
                  </svg>
                </span>
                <span className="rules-payment-method-name">Credit Cards</span>
              </li>
            </ul>
          </div>
        </div>
      </section>

      {/* Rule book — two-column shell with sticky rail + accordion */}
      <section className="faq-body">
        <RulesClient sections={rulesSections} />

        <div className="faq-footnote-wrap">
          <p className="faq-footnote">
            By participating in a Super6 Series LLC event, your team agrees to all rules
            and policies on this page. For non-rule questions, see the{" "}
            <Link href="/faq" className="faq-link">
              FAQ
            </Link>
            ; for anything else, use the{" "}
            <Link href="/contact" className="faq-link">
              Contact page
            </Link>
            .
          </p>
        </div>
      </section>

      {/* Final CTA */}
      <section className="faq-final-cta">
        <div className="container-xl">
          <p className="faq-final-cta-eyebrow">Read the rules.</p>
          <h2 className="faq-final-cta-title">Bring the team.</h2>
          <p className="faq-final-cta-sub">
            Now that you know how the weekend works, lock in your spot.
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
