/* ─── /gallery — the visual story of Super6 Series LLC.

   Server component: renders metadata + the static hero, then mounts the
   client GalleryGrid (which owns filter state and the lightbox).

   To edit images / categories / captions / years: see src/data/gallery.ts.
*/

import type { Metadata } from "next";
import Link from "next/link";
import GalleryGrid from "@/components/gallery/GalleryGrid";
import { REGISTER_LINK_PROPS } from "@/lib/links";
import { siteSmsHref } from "@/data/site";

export const metadata: Metadata = {
  title: "Gallery | Super6 Series LLC",
  description:
    "Thirteen years of Super6 Series LLC in pictures — tournament action, behind-the-scenes work, packed venues, champions, and the brand moments that built the league.",
  alternates: { canonical: "/gallery" },
  openGraph: {
    title: "Gallery | Super6 Series LLC",
    description:
      "Thirteen years of Super6 Series LLC in pictures — action, behind-the-scenes, packed venues, champions.",
    url: "/gallery",
    type: "website",
    images: [
      {
        url: "/media/hero/hero-dunk.jpg",
        width: 1600,
        height: 900,
        alt: "Super6 Series LLC gallery — tournament action and brand moments",
      },
    ],
  },
  twitter: {
    card: "summary_large_image",
    title: "Gallery | Super6 Series LLC",
    description:
      "Thirteen years of Super6 in pictures — action, venues, champions.",
    images: ["/media/hero/hero-dunk.jpg"],
  },
};

export default function GalleryPage() {
  return (
    <main>
      {/* ─── Filters + categorized sections + lightbox ─── */}
      <GalleryGrid />

      {/* ─── Closing CTA ─── */}
      <section className="gallery-cta" aria-label="Call to action">
        <div className="container-xl gallery-cta-inner">
          <p className="gallery-cta-eyebrow">Your team belongs here.</p>
          <h2 className="gallery-cta-heading">
            The next photo on this page <em>is yours.</em>
          </h2>
          <div className="gallery-cta-actions">
            <Link {...REGISTER_LINK_PROPS} className="btn btn-orange">
              Register your team
            </Link>
            <a href={siteSmsHref} className="btn btn-outline">
              Text Super6
            </a>
          </div>
        </div>
      </section>
    </main>
  );
}
