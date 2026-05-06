"use client";

import Image from "next/image";

/* ─── Editorial Hero
   Full-bleed photo; copy anchored bottom-right so faces stay open frame left.
   Serif stack + rail + restrained CTAs. */

export default function HeroSection() {
  return (
    <section className="editorial-hero editorial-hero--copy-br" aria-label="Hero">
      <div className="editorial-hero-media">
        <Image
          src="/media/uploads/celtics-super6.jpg"
          alt=""
          fill
          priority
          sizes="100vw"
          quality={92}
          style={{ objectFit: "cover", objectPosition: "28% 42%" }}
          aria-hidden="true"
        />
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
