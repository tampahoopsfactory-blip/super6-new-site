"use client";

import Image from "next/image";
import Link from "next/link";

/* ─── Editorial Hero
   Single full-bleed photograph with slow ken-burns motion.
   Eyebrow rule + serif headline + sub + restrained CTAs.
   Anthropic editorial calm × cinematic sports photography. */

export default function HeroSection() {
  return (
    <section className="editorial-hero" aria-label="Hero">
      <div className="editorial-hero-media">
        <Image
          src="/media/uploads/celtics-super6.jpg"
          alt=""
          fill
          priority
          sizes="100vw"
          quality={92}
          style={{ objectFit: "cover", objectPosition: "65% center" }}
          aria-hidden="true"
        />
      </div>

      {/* Floating brand mark, bottom-right of the hero */}
      <Image
        src="/media/logos/super6-mark-transparent.png"
        alt="Super 6"
        width={280}
        height={280}
        priority
        className="editorial-hero-mark"
      />

      <div className="editorial-hero-content">
        <span className="editorial-eyebrow">Florida · Georgia · Est. 2014</span>
        <h1 className="editorial-headline">
          Where champions <em>are made.</em>
        </h1>
        <p className="editorial-sub">
          Twelve years building the South’s most competitive youth basketball
          tournaments. NFHS-certified officials. College pipeline. A championship
          atmosphere every weekend.
        </p>
        <div className="editorial-actions">
          <Link href="/register" className="btn-hero btn-hero-primary">
            Register your team
          </Link>
          <Link href="/locations" className="btn-hero btn-hero-secondary">
            Explore tournaments
          </Link>
        </div>
      </div>
    </section>
  );
}
