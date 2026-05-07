"use client";

import Image from "next/image";

/* ─── Editorial Hero
   Two horizontal photo slots replacing the single dunk shot.
   Same headline, scrim, and height — only the media plate splits.
   Swap each slot independently by changing its `src`. */

const HERO_SLOTS = [
  {
    src: "/media/uploads/hero-dunk.jpg",
    alt: "",
    /* Anchor higher on frame — preserves hands / rim under the nav crop */
    objectPosition: "50% 24%",
    quality: 100,
  },
  {
    src: "/media/uploads/celtics-super6.jpg",
    alt: "",
    objectPosition: "32% 30%",
    quality: 100,
  },
] as const;

export default function HeroSection() {
  return (
    <section className="editorial-hero editorial-hero--copy-br" aria-label="Hero">
      <div className="editorial-hero-media editorial-hero-media--split" aria-hidden="true">
        {HERO_SLOTS.map((slot, i) => (
          <div className="editorial-hero-slot" key={slot.src}>
            <Image
              src={slot.src}
              alt={slot.alt}
              fill
              priority={i === 0}
              sizes="(max-width: 700px) 100vw, 50vw"
              quality={slot.quality}
              style={{ objectFit: "cover", objectPosition: slot.objectPosition }}
            />
          </div>
        ))}
      </div>

      <div className="editorial-hero-content editorial-hero-content--br">
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
