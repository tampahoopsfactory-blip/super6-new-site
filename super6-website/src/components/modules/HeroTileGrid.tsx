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

      <div className="editorial-hero-content">
        <span className="editorial-eyebrow">
          Florida · Georgia · Since 2014
        </span>
        <h1 className="editorial-headline">
          <span className="brand-mark">SUPER6.</span> Where champions{" "}
          <em>are made.</em>
        </h1>
        <p className="editorial-kicker">
          The South&apos;s standard for youth basketball.
        </p>
        <p className="editorial-sub">
          Twelve years building the most competitive youth weekends in two
          states — NFHS-certified officiating, a real college pipeline, and
          championship atmosphere from the first tip to the last whistle.
        </p>
        <p className="editorial-sub-tag">
          Boys and girls · 3rd–12th grade · Every event, every weekend
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
