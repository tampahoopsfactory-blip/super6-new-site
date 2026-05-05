"use client";

import Image from "next/image";
import Link from "next/link";

/* ─── Event Security spread
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
    title: "Trained security personnel",
    body:
      "Licensed officers at every gate, on the floor, and in the lot. The same crew that reads the room before it turns — diffusing tension, redirecting energy, and stepping in long before anything flares up.",
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
      aria-label="Event security at Super 6"
    >
      <div className="security-spread-stage">
        <div className="security-spread-visual">
          <Image
            src="/media/uploads/security-bag-check-full.png"
            alt="Security officer inspecting a bag at the venue entrance during a Super 6 tournament"
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
              Gates · Courts · Parking · Lots
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
              Professional event security <em>at every Super 6 weekend.</em>
            </h2>
            <p className="security-spread-subhead-plain">
              Licensed personnel at entries, on the floor, and in parking areas —{" "}
              <strong>every Super 6 weekend</strong>.
            </p>
            <p className="security-spread-lede">
              Every bag checked. Every entrance covered. Security is a Super 6 priority — our staff
              keep events safe, calm, and orderly while hundreds of families, athletes, and staff
              share one building — the standard your families expect when they arrive.
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
                </svg>
              </div>
              <div>
                <p className="security-spread-signature-eyebrow">Security standard</p>
                <p className="security-spread-signature-name">Contracted event security</p>
                <p className="security-spread-signature-tag">
                  Professional coverage at every Super 6 venue.
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
