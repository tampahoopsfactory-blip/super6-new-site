import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";
import FAQClient from "./_components/FAQClient";
import {
  buildFaqJsonLd,
  faqIntro,
  faqSections,
  totalQuestions,
} from "./faq-data";
import { REGISTER_LINK_PROPS } from "@/lib/links";
import { siteSmsHref } from "@/data/site";

/* ─── /faq — server component
   - Editorial hero (kept from previous design, spacing tightened below)
   - JSON-LD FAQPage schema injected for SEO
   - Two-column shell handled by FAQClient (sticky nav + accordion)
*/

export const metadata: Metadata = {
  title: "FAQ | Super6 Series LLC",
  description:
    "Answers to the most common Super6 Series LLC questions — registration, the Super6 Series LLC app, schedule, game rules, eligibility, gate security, officials, venues, and refunds.",
  alternates: { canonical: "/faq" },
  openGraph: {
    title: "FAQ | Super6 Series LLC",
    description:
      "Registration, app, schedule, rules, gate security, officials, venues, refunds — all in one place.",
    url: "/faq",
    type: "website",
    images: [
      {
        url: "/media/curated/faq-hero-shooter.png",
        width: 1600,
        height: 900,
        alt: "Super6 Series LLC FAQ — answers for coaches and families",
      },
    ],
  },
  twitter: {
    card: "summary_large_image",
    title: "FAQ | Super6 Series LLC",
    description:
      "Every question coaches and families actually ask — answered.",
    images: ["/media/curated/faq-hero-shooter.png"],
  },
};

export default function FAQPage() {
  const jsonLd = buildFaqJsonLd();

  return (
    <>
      {/* JSON-LD FAQPage schema */}
      <script
        type="application/ld+json"
        dangerouslySetInnerHTML={{ __html: JSON.stringify(jsonLd) }}
      />

      {/* Hero — Editorial split, mirrors rules / contact / officials */}
      <section className="faq-hero">
        <div className="faq-hero-photo">
          <Image
            src="/media/curated/faq-hero-shooter.png"
            alt="Super6 player squaring up to shoot in front of a packed Florida Hospital gym crowd."
            fill
            priority
            quality={94}
            sizes="(max-width: 968px) 100vw, 55vw"
            style={{ objectFit: "cover", objectPosition: "center 38%" }}
          />
        </div>

        <div className="faq-hero-panel">
          <div className="faq-hero-meta">
            <span className="faq-hero-meta-tag">FAQ</span>
            <span className="faq-hero-meta-divider" />
            <span>2026 SEASON · COACHES &amp; FAMILIES</span>
          </div>

          <div className="faq-hero-wordmark" aria-hidden="true">
            <span className="faq-hero-word-primary">ASKED</span>
            <span className="faq-hero-word-accent">&amp; answered.</span>
            <span className="faq-hero-word-tag">{faqIntro.eyebrow}</span>
          </div>

          <h1 className="faq-hero-headline">
            Everything coaches and families <em>actually ask.</em>
          </h1>

          <p className="faq-hero-desc">
            {faqIntro.body.replace("/rules", "")}{" "}
            <Link href="/rules" className="faq-link faq-link--inverse">
              Read the rule book →
            </Link>
          </p>

          <div className="faq-hero-index">
            <span className="faq-hero-index-num">
              {String(totalQuestions).padStart(2, "0")}
            </span>
            <div className="faq-hero-index-text">
              <span className="faq-hero-index-label">Questions Answered</span>
              <span className="faq-hero-index-sub">
                {faqSections.length} sections — getting started, the app,
                schedule, rules, check-in, officials, venues, refunds.
              </span>
            </div>
          </div>
        </div>
      </section>

      {/* FAQ body — two-column shell with sticky nav */}
      <section className="faq-body">
        <FAQClient sections={faqSections} />

        <div className="faq-footnote-wrap">
          <p className="faq-footnote">
            By participating in a Super6 event, your team agrees to all rules
            and policies on{" "}
            <Link href="/rules" className="faq-link">
              thesuper6.com/rules
            </Link>
            . When in doubt, the official rules page is authoritative.
          </p>
        </div>
      </section>

      {/* Final CTA */}
      <section className="faq-final-cta">
        <div className="container-xl">
          <p className="faq-final-cta-eyebrow">Still have a question?</p>
          <h2 className="faq-final-cta-title">
            We answer same-day, every weekday.
          </h2>
          <p className="faq-final-cta-sub">
            Real-time event questions belong in the Super6 app. Anything else —
            registration, billing, eligibility — comes through here.
          </p>
          <div className="faq-final-cta-actions">
            <a href={siteSmsHref} className="btn-hero btn-hero-primary">
              Text Super6
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
