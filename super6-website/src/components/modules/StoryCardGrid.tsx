"use client";

import Image from "next/image";
import Link from "next/link";

/* ─── Event Security — Big Kelly's BKS
   Full-viewport-height split: full-bleed gate photo (cover) | editorial column.
   Rules CTA sits in container below. */

const protocols = [
  {
    num: "01",
    title: "Every Bag Inspected",
    body:
      "Bag checks at every gate, every entry. We screen for weapons, contraband, and anything that doesn't belong inside a youth event.",
  },
  {
    num: "02",
    title: "Trained BKS Personnel",
    body:
      "Licensed BKS officers at every gate, on the floor, and in the lot. The same crew that reads the room before it turns—diffusing tension, redirecting energy, and stepping in long before anything flares up.",
  },
  {
    num: "03",
    title: "Zero-Tolerance Enforcement",
    body:
      "Disrespect, threats, or unauthorized entry meet immediate removal. Every family feels the difference the second they walk in.",
  },
];

export default function SecuritySpread() {
  return (
    <section
      className="security-spread"
      aria-label="Event security at Super 6 — Big Kelly's BKS Security"
    >
      <div className="security-spread-stage">
        <div className="security-spread-visual">
          <Image
            src="/media/uploads/security-bag-check-full.png"
            alt="BKS Security officer inspecting a bag at the venue entrance during a Super 6 tournament"
            fill
            sizes="(max-width: 968px) 100vw, 46vw"
            quality={95}
            priority={false}
            className="security-spread-visual-img"
            style={{ objectFit: "cover", objectPosition: "center center" }}
          />
          <div className="security-spread-visual-footer" aria-hidden="true">
            <span className="security-spread-visual-footer-tag">Live</span>
            <span className="security-spread-visual-footer-text">
              Big Kelly&apos;s BKS · Gate · Bag · Presence
            </span>
          </div>
        </div>

        <div className="security-spread-copy">
          <div className="security-spread-content">
            <p className="security-spread-eyebrow">
              <span className="security-spread-eyebrow-rule" aria-hidden="true" />
              Why Super 6 · Event Security
            </p>
            <h2 className="security-spread-headline">
              No one watches the room <em>like Big Kelly.</em>
            </h2>
            <p className="security-spread-subhead">
              Safety <em>before the spark.</em>
            </p>
            <p className="security-spread-lede">
              Every bag checked. Every entrance covered. Security is a Super 6 priority — we{" "}
              <strong>don&apos;t run a weekend without</strong>{" "}
              <strong>Big Kelly&apos;s BKS Security</strong>. Their officers keep our events safe,
              calm, and orderly while hundreds of families, athletes, and staff share one building —
              the serious standard your families feel the second they walk in.
            </p>

            <ol className="security-spread-protocols">
              {protocols.map((p) => (
                <li key={p.num}>
                  <span className="security-spread-protocol-num" aria-hidden="true">
                    {p.num}
                  </span>
                  <div>
                    <h3 className="security-spread-protocol-title">{p.title}</h3>
                    <p className="security-spread-protocol-body">{p.body}</p>
                  </div>
                </li>
              ))}
            </ol>

            <div className="security-spread-btn-row">
              <Link href="/about" className="btn btn-ink">
                Meet the team
              </Link>
            </div>

            <div className="security-spread-signature">
              <div className="security-spread-signature-shield" aria-hidden="true">
                <svg viewBox="0 0 48 56" width="40" height="46" fill="none">
                  <path
                    d="M24 2L4 10v18c0 13 8 21 20 26 12-5 20-13 20-26V10L24 2z"
                    stroke="currentColor"
                    strokeWidth="1.5"
                  />
                  <text
                    x="24"
                    y="32"
                    textAnchor="middle"
                    fontFamily="var(--font-display)"
                    fontSize="11"
                    fontWeight="700"
                    fill="currentColor"
                  >
                    BKS
                  </text>
                </svg>
              </div>
              <div>
                <p className="security-spread-signature-eyebrow">Security Partner</p>
                <p className="security-spread-signature-name">
                  Big Kelly&apos;s BKS Security Group
                </p>
                <p className="security-spread-signature-tag">
                  Contracted to keep every Super 6 event safe.
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="container-xl security-spread-rules-wrap">
        <Link href="/rules" className="security-spread-rules-cta">
          <div className="security-spread-rules-cta-mark" aria-hidden="true">
            <span className="security-spread-rules-cta-eyebrow">Tournament Rule Book</span>
            <span className="security-spread-rules-cta-title">Game Rules</span>
          </div>
          <p className="security-spread-rules-cta-body">
            Conduct, gate security, uniform compliance, ejections, forfeits — every line governing
            Super 6 play. Read before you register.
          </p>
          <span className="security-spread-rules-cta-arrow" aria-hidden="true">
            <svg
              width="32"
              height="32"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="1.5"
            >
              <path d="M5 12h14M13 5l7 7-7 7" />
            </svg>
          </span>
        </Link>
      </div>
    </section>
  );
}
