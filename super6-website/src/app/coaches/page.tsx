import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";
import { REGISTER_LINK_PROPS } from "@/lib/links";
import { siteSmsHref } from "@/data/site";
import SectionIcon from "../rules/_components/SectionIcon";
import {
  coachesIntro,
  jobDescriptionSection,
  jobDescriptionItems,
  billSection,
  billItems,
  cutSection,
  cutBody,
  builtDifferentSection,
  pillars,
  numbers,
  wordOfMouthSection,
  wordOfMouthBody,
  thesis,
  finalCta,
} from "./coaches-data";

/* ─── /coaches — Coaches appreciation + recruitment.
   Server-rendered editorial article. Mirrors the /rules visual system
   in the hero (.rules-hero*) and final CTA (.faq-final-cta*), but the
   body uses full-bleed 50/50 splits (.coaches-split) for editorial sections
   and the closing thesis. The hero is preserved verbatim.

   Sections 01–05 + thesis share the .coaches-split component:
   a full-bleed band; default layout is copy left / photo right; sections
   02 and 04 pass imageFirst; thesis adds coaches-split--image-first —
   all photo left / copy right on desktop. Tints alternate
   between cream and cream-warm so the page reads as a rhythm. The
   thesis uses the same split plus coaches-split--image-first (photo
   left / copy right, matching sections 02 & 04). The Numbers strip and Final CTA
   bands are unchanged. */

export const metadata: Metadata = {
  title: "For Coaches",
  description:
    "Affordable youth basketball and volleyball tournaments built for the coaches who do everything. 400+ tournaments. 12 years. Florida and Georgia. Keep your team together longer with Super6 Series LLC.",
  alternates: { canonical: "/coaches" },
  keywords: [
    "affordable basketball tournaments for coaches",
    "youth basketball tournaments Florida",
    "youth volleyball tournaments Florida",
    "coaches youth basketball league",
    "Super6 Series LLC coaches",
    "Florida youth basketball league",
    "Georgia youth basketball tournaments",
  ],
  openGraph: {
    title: "To the coaches who do everything | Super6 Series LLC",
    description:
      "We see you. Affordable, high-quality tournaments built for the coaches who keep teams together.",
    url: "/coaches",
    type: "website",
    images: ["/media/uploads/coaches-hero-new.png"],
  },
};

const orgJsonLd = {
  "@context": "https://schema.org",
  "@type": "SportsOrganization",
  name: "Super6 Series LLC",
  url: "https://www.thesuper6.com",
  sameAs: [
    "https://www.instagram.com/super6florida/",
    "https://twitter.com/TheSuper6Series",
    "https://www.facebook.com/thesuper6series/",
  ],
  description:
    "Super6 Series LLC runs affordable, high-quality youth basketball tournaments across Florida and Georgia, built for the coaches who keep teams together.",
  areaServed: ["FL", "GA"],
  sport: ["Basketball", "Volleyball"],
};

export default function CoachesPage() {
  return (
    <>
      {/* JSON-LD SportsOrganization schema */}
      <script
        type="application/ld+json"
        dangerouslySetInnerHTML={{ __html: JSON.stringify(orgJsonLd) }}
      />

      {/* ─── Hero — same .rules-hero editorial split, recomposed copy ─── */}
      <section className="rules-hero coaches-hero">
        <div className="rules-hero-photo">
          <Image
            src="/media/uploads/coaches-hero-new.png"
            alt=""
            fill
            priority
            quality={100}
            sizes="(max-width: 968px) 100vw, 55vw"
            aria-hidden="true"
          />
        </div>

        <div className="rules-hero-panel">
          <div className="rules-hero-meta">
            <span className="rules-hero-meta-tag">
              {coachesIntro.eyebrow.toUpperCase()}
            </span>
            <span className="rules-hero-meta-divider" />
            <span>{coachesIntro.meta}</span>
          </div>

          <div className="rules-hero-wordmark" aria-hidden="true">
            <span className="rules-hero-word-game">
              {coachesIntro.wordmarkTop}
            </span>
            <span className="rules-hero-word-rules">
              {coachesIntro.wordmarkAccent}
            </span>
            <span className="rules-hero-word-book">
              {coachesIntro.wordmarkLabel}
            </span>
          </div>

          <h1 className="rules-hero-headline">
            {coachesIntro.headline.split(" ").slice(0, -1).join(" ")}{" "}
            <em>{coachesIntro.headline.split(" ").slice(-1)[0]}</em>
          </h1>

          <p className="rules-hero-desc">{coachesIntro.desc}</p>

          <div className="rules-hero-index">
            <span className="rules-hero-index-num">
              {coachesIntro.index.num}
            </span>
            <div className="rules-hero-index-text">
              <span className="rules-hero-index-label">
                {coachesIntro.index.label}
              </span>
              <span className="rules-hero-index-sub">
                {coachesIntro.index.sub}
              </span>
            </div>
          </div>
        </div>
      </section>

      {/* ─── Section 01 — The job description nobody wrote ─── */}
      <CoachesSplitSection section={jobDescriptionSection}>
        <ol className="coaches-statements" aria-label="The job description">
          {jobDescriptionItems.map((s) => (
            <li key={s.num} className="coaches-statement">
              <span className="coaches-statement-num" aria-hidden="true">
                {s.num}
              </span>
              <span className="coaches-statement-rule" aria-hidden="true" />
              <span className="coaches-statement-body">{s.body}</span>
            </li>
          ))}
        </ol>
      </CoachesSplitSection>

      {/* ─── Section 02 — The bill nobody talks about (image left) ─── */}
      <CoachesSplitSection section={billSection} imageFirst>
        <p className="coaches-lead">
          Gym time. Tournament fees. Uniforms. Travel. Equipment. Refs.
          Subs.
        </p>
        <ol className="coaches-statements coaches-statements--compact">
          {billItems.map((s) => (
            <li key={s.num} className="coaches-statement">
              <span className="coaches-statement-num" aria-hidden="true">
                {s.num}
              </span>
              <span className="coaches-statement-rule" aria-hidden="true" />
              <span className="coaches-statement-body">{s.body}</span>
            </li>
          ))}
        </ol>
        <p className="coaches-aside">
          Team dues never cover the whole season. So coaches cover the
          gap. Out of pocket. Quietly. Every year.
        </p>
      </CoachesSplitSection>

      {/* ─── Section 03 — The cut that hurts the most ─── */}
      <CoachesSplitSection section={cutSection}>
        <ul
          className="coaches-split-bullets coaches-split-bullets--cut"
          aria-label="The cut that hurts the most"
        >
          {cutBody.map((line, i) => (
            <li
              key={i}
              className={
                i === cutBody.length - 1
                  ? "coaches-split-bullets-final"
                  : undefined
              }
            >
              {line}
            </li>
          ))}
        </ul>
      </CoachesSplitSection>

      {/* ─── Section 04 — Super6 was built different (image left) ─── */}
      <CoachesSplitSection section={builtDifferentSection} imageFirst>
        <ul className="coaches-pillars" aria-label="What makes Super6 different">
          {pillars.map((p) => (
            <li key={p.tag} className="coaches-pillar">
              <p className="coaches-pillar-tag">{p.tag}</p>
              <p className="coaches-pillar-body">{p.body}</p>
            </li>
          ))}
        </ul>
      </CoachesSplitSection>

      {/* ─── Numbers strip — full-bleed dark band (unchanged) ─── */}
      <section className="coaches-numbers" aria-label="Super6 by the numbers">
        <div className="container-xl">
          <ul className="coaches-numbers-grid">
            {numbers.map((n) => (
              <li key={n.label} className="coaches-numbers-cell">
                <span className="coaches-numbers-num">{n.value}</span>
                <span className="coaches-numbers-label">{n.label}</span>
              </li>
            ))}
          </ul>
        </div>
      </section>

      {/* ─── Section 05 — Why coaches keep coming back ─── */}
      <CoachesSplitSection section={wordOfMouthSection}>
        <ul
          className="coaches-split-bullets"
          aria-label="Why coaches keep coming back"
        >
          {wordOfMouthBody.map((text, i) => (
            <li key={i}>{text}</li>
          ))}
        </ul>
        {/* TESTIMONIALS PLACEHOLDER */}
      </CoachesSplitSection>

      {/* ─── Thesis — Keep the team. Keep the kid. (split layout) ─── */}
      <section
        id="thesis"
        className="coaches-split coaches-split--alt coaches-split--thesis coaches-split--image-first"
        aria-labelledby="coaches-thesis-title"
      >
        <div className="coaches-split-grid">
          <div className="coaches-split-content">
            <div className="coaches-split-content-inner">
              <p className="coaches-thesis-eyebrow">The thesis</p>
              <h2
                id="coaches-thesis-title"
                className="coaches-thesis-headline"
              >
                Keep the team. <em>Keep the kid.</em>
              </h2>

              <ol
                className="coaches-thesis-stair"
                aria-label="The thesis, step by step"
              >
                {thesis.stair.map((line, i) => (
                  <li
                    key={i}
                    className="coaches-thesis-line"
                    style={{ ["--coaches-stair-index" as string]: i }}
                  >
                    {line}
                  </li>
                ))}
              </ol>

              <p className="coaches-thesis-close">{thesis.close}</p>
            </div>
          </div>

          <div className="coaches-split-image">
            <Image
              src={thesis.images[0].src}
              alt={thesis.images[0].alt}
              fill
              quality={100}
              sizes="(max-width: 968px) 100vw, 50vw"
              style={{ objectFit: "cover", objectPosition: "center" }}
            />
          </div>
        </div>
      </section>

      {/* ─── Final CTA — same band as /rules and /faq ─── */}
      <section className="faq-final-cta">
        <div className="container-xl">
          <p className="faq-final-cta-eyebrow">{finalCta.eyebrow}</p>
          <h2 className="faq-final-cta-title">{finalCta.title}</h2>
          <p className="faq-final-cta-sub">{finalCta.sub}</p>
          <div className="faq-final-cta-actions">
            <Link
              {...REGISTER_LINK_PROPS}
              className="btn-hero btn-hero-primary"
            >
              Register a team
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

/* ─── Full-bleed editorial split.
   Default: copy column left, photo right (DOM: content then image).
   Pass imageFirst to flip desktop columns (photo left, copy right) via
   grid item order — keeps one markup shape for LCP / hydration.

   Background tint alternates by section number parity:
     odd  (01, 03, 05) → base cream
     even (02, 04)     → cream-warm */
function CoachesSplitSection({
  section,
  children,
  imageFirst = false,
  imageObjectFit = "cover",
}: {
  section: {
    id: string;
    number: string;
    title: string;
    description?: string;
    icon: Parameters<typeof SectionIcon>[0]["name"];
    image?: {
      src: string;
      alt: string;
      cropLetterbox?: boolean;
    };
  };
  children: React.ReactNode;
  /** Photo in the left column on desktop (and stacked first on mobile). */
  imageFirst?: boolean;
  /** Use `contain` when the photo must not be cropped (letterboxing inside the frame). */
  imageObjectFit?: "cover" | "contain";
}) {
  const isEven = Number.parseInt(section.number, 10) % 2 === 0;
  const className =
    "coaches-split" +
    (isEven ? " coaches-split--alt" : "") +
    (imageFirst ? " coaches-split--image-first" : "");
  const titleId = `coaches-section-${section.id}-title`;

  return (
    <section
      id={section.id}
      className={className}
      aria-labelledby={titleId}
    >
      <div className="coaches-split-grid">
        {/* Copy first so in the two-column grid it anchors left and the
           photo sits right (grid column order follows DOM order). */}
        <div className="coaches-split-content">
          <div className="coaches-split-content-inner">
            <header className="faq-section-header coaches-split-header">
              <span className="faq-section-watermark" aria-hidden="true">
                {section.number}
              </span>

              <div className="faq-section-icon">
                <SectionIcon name={section.icon} size={22} strokeWidth={1.8} />
              </div>

              <div className="faq-section-titlewrap">
                <p className="faq-section-eyebrow">
                  <span className="faq-section-eyebrow-rule" aria-hidden="true" />
                  Section {section.number}
                </p>
                <h2 id={titleId} className="faq-section-title">
                  {section.title}
                </h2>
                {section.description ? (
                  <p className="faq-section-desc">{section.description}</p>
                ) : null}
              </div>
            </header>

            <div className="coaches-split-body">{children}</div>
          </div>
        </div>

        {section.image ? (
          <div
            className={
              "coaches-split-image" +
              (imageObjectFit === "contain"
                ? " coaches-split-image--contain"
                : "") +
              (section.image.cropLetterbox
                ? " coaches-split-image--crop-letterbox"
                : "")
            }
          >
            <Image
              src={section.image.src}
              alt={section.image.alt}
              fill
              quality={100}
              sizes="(max-width: 968px) 100vw, 50vw"
              style={{
                objectFit: imageObjectFit,
                objectPosition: section.image.cropLetterbox
                  ? "center 36%"
                  : "center",
              }}
            />
          </div>
        ) : null}
      </div>
    </section>
  );
}
