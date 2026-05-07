import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";
import SecurityClient from "./_components/SecurityClient";
import {
  securityIntro,
  securitySections,
  totalSecurityItems,
} from "./security-data";
import { REGISTER_LINK_PROPS } from "@/lib/links";
import { siteSmsHref } from "@/data/site";

export const metadata: Metadata = {
  title: "Event Security",
  description:
    "Super6 Series LLC event security standard. Licensed personnel at entries, on the floor, and in parking areas. Bag checks, weapons screening, zero-tolerance enforcement.",
  alternates: { canonical: "/security" },
  openGraph: {
    title: "Event Security | Super6 Series LLC",
    description:
      "Licensed personnel, bag checks, zero-tolerance enforcement — the Super6 event security standard.",
    url: "/security",
    type: "website",
    images: [
      {
        url: "/media/uploads/security-bag-check-full.png",
        width: 1600,
        height: 900,
        alt: "Super6 security officer inspecting a bag at the venue entrance",
      },
    ],
  },
  twitter: {
    card: "summary_large_image",
    title: "Event Security | Super6 Series LLC",
    description:
      "The Super6 event security standard. Licensed crew at every gate, every weekend.",
    images: ["/media/uploads/security-bag-check-full.png"],
  },
};

export default function SecurityPage() {
  return (
    <>
      {/* Hero — Editorial treatment mirroring /rules */}
      <section className="rules-hero">
        <div className="rules-hero-photo">
          <Image
            src="/media/uploads/security-bag-check-full.png"
            alt=""
            fill
            priority
            quality={94}
            sizes="(max-width: 968px) 100vw, 55vw"
            aria-hidden="true"
            style={{ objectFit: "cover", objectPosition: "center center" }}
          />
        </div>

        <div className="rules-hero-panel">
          <div className="rules-hero-meta">
            <span className="rules-hero-meta-tag">
              {securityIntro.eyebrow.toUpperCase()}
            </span>
            <span className="rules-hero-meta-divider" />
            <span>LICENSED · CONTRACTED · ON-SITE</span>
          </div>

          <div className="rules-hero-wordmark" aria-hidden="true">
            <span className="rules-hero-word-game">EVENT</span>
            <span className="rules-hero-word-rules">
              SECURITY
              <em className="rules-hero-amp">&</em>
            </span>
            <span className="rules-hero-word-book">SAFETY</span>
          </div>

          <h1 className="rules-hero-headline">
            Professional event security <em>at every Super6 weekend.</em>
          </h1>

          <p className="rules-hero-desc">
            {securityIntro.body}{" "}
            <Link href="/rules" className="faq-link faq-link--inverse">
              Read the rule book →
            </Link>
          </p>

          <div className="rules-hero-index">
            <span className="rules-hero-index-num">
              {String(totalSecurityItems).padStart(2, "0")}
            </span>
            <div className="rules-hero-index-text">
              <span className="rules-hero-index-label">
                Standards in {securitySections.length} sections
              </span>
              <span className="rules-hero-index-sub">
                Personnel · Entrance protocol · Floor & crowd · Enforcement
              </span>
            </div>
          </div>
        </div>
      </section>

      {/* Body — two-column shell with sticky rail + accordion */}
      <section className="faq-body">
        <SecurityClient sections={securitySections} />

        <div className="faq-footnote-wrap">
          <p className="faq-footnote">
            By attending a Super6 event, all spectators, athletes, and staff
            agree to the security standard on this page. For tournament rules,
            see the{" "}
            <Link href="/rules" className="faq-link">
              rule book
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
          <p className="faq-final-cta-eyebrow">Safe weekends, every time.</p>
          <h2 className="faq-final-cta-title">Bring the team.</h2>
          <p className="faq-final-cta-sub">
            The standard your families expect, the moment they arrive.
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
