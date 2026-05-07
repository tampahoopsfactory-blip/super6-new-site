"use client";

import Image from "next/image";

/* ─── Editorial Hero — single photo (LOCKED).
   The homepage hero is a single full-bleed photograph: the Celtics #7
   matchup at /media/uploads/celtics-super6.jpg. Do not switch back to a
   split / triptych / multi-slot layout without TK explicit approval.

   See:
   - CLAUDE.md / .cursorrules → "Homepage hero — locked single photo"
   - .cursor/rules/home-hero-single-photo.mdc

   Tunables are limited to this file: objectPosition (focal anchor),
   quality, and the headline copy. Photo file path itself is locked. */

const HERO_IMAGE = {
  src: "/media/uploads/celtics-super6.jpg",
  alt: "",
  /* objectPosition — anchors the dribbler (Celtics #7) and the defender
     in frame across viewport widths. "50%" keeps both players centered.
     "30%" keeps heads/faces above the bottom-of-frame scrim. */
  objectPosition: "50% 30%",
  quality: 100,
} as const;

export default function HeroSection() {
  return (
    <section
      className="editorial-hero editorial-hero--copy-bc"
      aria-label="Hero"
    >
      <div className="editorial-hero-media" aria-hidden="true">
        <Image
          src={HERO_IMAGE.src}
          alt={HERO_IMAGE.alt}
          fill
          priority
          sizes="100vw"
          quality={HERO_IMAGE.quality}
          style={{
            objectFit: "cover",
            objectPosition: HERO_IMAGE.objectPosition,
          }}
        />
      </div>

      <div className="editorial-hero-content editorial-hero-content--bc">
        <h1 className="editorial-headline hero-headline">
          <span className="hero-headline-taglock">
            <span className="hero-tagline-line hero-tagline-line--cream">
              Where Champions
            </span>
            <span className="hero-tagline-line hero-tagline-line--accent">
              Are Made&hellip;
            </span>
          </span>
        </h1>
        <p className="hero-meta">FL · GA · Since 2014</p>
      </div>
    </section>
  );
}
