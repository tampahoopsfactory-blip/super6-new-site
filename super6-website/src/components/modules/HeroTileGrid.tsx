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
          src="/media/curated/04-defensive-stance.jpg"
          alt=""
          fill
          priority
          sizes="100vw"
          quality={88}
          style={{ objectFit: "cover", objectPosition: "center 35%" }}
          aria-hidden="true"
        />
      </div>

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
