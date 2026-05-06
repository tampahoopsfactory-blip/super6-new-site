"use client";

import Image from "next/image";

/* ─── Editorial Hero
   Full-bleed photo; copy anchored bottom-right so faces stay open frame left.
   Serif stack + rail + restrained CTAs. */

const heroSections = [
  {
    src: "/media/uploads/hero-dunk.jpg",
    position: "center 35%",
  },
  {
    src: "/media/uploads/celtics-super6.jpg",
    position: "34% 42%",
  },
  {
    src: "/media/uploads/boys-girls-division-balanced-2026.png",
    position: "center 44%",
  },
];

export default function HeroSection() {
  return (
    <section className="editorial-hero editorial-hero--copy-br" aria-label="Hero">
      <div className="editorial-hero-media editorial-hero-media--section-row">
        {heroSections.map((photo) => (
          <article className="editorial-hero-photo-section" key={photo.src}>
            <Image
              src={photo.src}
              alt=""
              fill
              priority
              sizes="(max-width: 700px) 100vw, 34vw"
              quality={100}
              style={{ objectFit: "cover", objectPosition: photo.position }}
              aria-hidden="true"
            />
          </article>
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
