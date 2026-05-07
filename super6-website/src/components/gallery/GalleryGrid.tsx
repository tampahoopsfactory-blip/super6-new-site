"use client";

/* ─── GalleryGrid — main interactive surface.

   Holds:
     - active filter tab (All or one category)
     - lightbox open state (index into the *currently visible* list)

   Renders:
     - Filter tab bar (sticky under nav)
     - When "All": category sections interleaved with cinematic
       spotlight banners + a Featured Year photo essay + the
       Through-the-Years rail. Section themes (dark vs cream)
       come from SECTION_THEME in src/data/gallery.ts.
     - When a category is selected: just that one section.
     - Lightbox overlaid when any photo is clicked.

   The lightbox always navigates within whatever list is currently
   visible — so prev/next stays in-context with the active filter.
*/

import { useMemo, useState } from "react";
import Image from "next/image";
import GalleryItem from "./GalleryItem";
import Lightbox from "./Lightbox";
import {
  PHOTOS,
  CATEGORIES,
  CATEGORY_META,
  TIMELINE,
  SECTION_THEME,
  SPOTLIGHTS,
  type GalleryCategory,
  type GalleryPhoto,
  type Spotlight,
} from "@/data/gallery";

type FilterId = GalleryCategory | "all";

export default function GalleryGrid() {
  const [filter, setFilter] = useState<FilterId>("all");
  const [lightboxIndex, setLightboxIndex] = useState<number | null>(null);

  // What the lightbox navigates over: filtered list when a category is active,
  // otherwise the full ordered list.
  const visiblePhotos = useMemo<GalleryPhoto[]>(() => {
    if (filter === "all") return PHOTOS;
    return PHOTOS.filter((p) => p.category === filter);
  }, [filter]);

  // Photos grouped by category for the section-stacked view
  const grouped = useMemo(() => {
    const map: Record<GalleryCategory, GalleryPhoto[]> = {
      "tournament-action":  [],
      "behind-the-scenes":  [],
      "venues-facilities":  [],
      "athletes-champions": [],
      "brand-moments":      [],
    };
    for (const p of PHOTOS) map[p.category].push(p);
    return map;
  }, []);

  const open = (photo: GalleryPhoto) => {
    const idx = visiblePhotos.findIndex((p) => p.id === photo.id);
    if (idx >= 0) setLightboxIndex(idx);
  };

  const close = () => setLightboxIndex(null);
  const next = () => {
    setLightboxIndex((i) => (i === null ? null : (i + 1) % visiblePhotos.length));
  };
  const prev = () => {
    setLightboxIndex((i) =>
      i === null ? null : (i - 1 + visiblePhotos.length) % visiblePhotos.length,
    );
  };

  // For each category, the list of spotlights that should render after it
  const spotlightsAfter = useMemo(() => {
    const m: Record<GalleryCategory, Spotlight[]> = {
      "tournament-action":  [],
      "behind-the-scenes":  [],
      "venues-facilities":  [],
      "athletes-champions": [],
      "brand-moments":      [],
    };
    for (const s of SPOTLIGHTS) m[s.placeAfter].push(s);
    return m;
  }, []);

  return (
    <>
      {/* ─── Filter bar ─── */}
      <nav aria-label="Gallery filters" className="gallery-filterbar">
        <div className="container-xl">
          <ul className="gallery-filters">
            {CATEGORIES.map((c) => (
              <li key={c.id}>
                <button
                  type="button"
                  className={`gallery-filter${filter === c.id ? " is-active" : ""}`}
                  onClick={() => setFilter(c.id)}
                  aria-pressed={filter === c.id}
                >
                  {c.label}
                </button>
              </li>
            ))}
          </ul>
        </div>
      </nav>

      {/* ─── Sections ─── */}
      {filter === "all" ? (
        <>
          {/* Through the Years opens the page — the league's story first. */}
          <TimelineSection onOpenPhoto={open} />

          {(Object.keys(grouped) as GalleryCategory[]).map((cat) => (
            <ScopeFragment key={cat}>
              <CategorySection
                category={cat}
                photos={grouped[cat]}
                onOpenPhoto={open}
              />

              {/* Cinematic full-bleed spotlight banners after this category */}
              {spotlightsAfter[cat].map((s) => (
                <SpotlightBanner key={s.id} spotlight={s} />
              ))}
            </ScopeFragment>
          ))}
        </>
      ) : (
        <CategorySection
          category={filter}
          photos={visiblePhotos}
          onOpenPhoto={open}
        />
      )}

      {/* ─── Lightbox ─── */}
      <Lightbox
        photos={visiblePhotos}
        index={lightboxIndex}
        onClose={close}
        onPrev={prev}
        onNext={next}
      />
    </>
  );
}

/* Tiny wrapper so we can return multiple siblings inside .map without
   an unkeyed Fragment warning. */
function ScopeFragment({ children }: { children: React.ReactNode }) {
  return <>{children}</>;
}

/* ─────────── Category section ─────────── */

function CategorySection({
  category,
  photos,
  onOpenPhoto,
}: {
  category: GalleryCategory;
  photos: GalleryPhoto[];
  onOpenPhoto: (p: GalleryPhoto) => void;
}) {
  const meta = CATEGORY_META[category];
  const theme = SECTION_THEME[category];
  return (
    <section
      id={category}
      aria-labelledby={`${category}-h`}
      className={`gallery-section ${theme === "dark" ? "gallery-section--dark" : ""}`}
    >
      <div className="container-xl">
        <header className="gallery-section-head">
          <p className="gallery-section-eyebrow">{meta.eyebrow}</p>
          <h2 id={`${category}-h`} className="gallery-section-heading">
            {meta.label}
          </h2>
          <p className="gallery-section-blurb">{meta.blurb}</p>
        </header>

        <div className="gallery-masonry">
          {photos.map((photo, i) => (
            <GalleryItem
              key={photo.id}
              photo={photo}
              index={i}
              onOpen={() => onOpenPhoto(photo)}
              priority={i < 2}
            />
          ))}
        </div>
      </div>
    </section>
  );
}

/* ─────────── Cinematic full-bleed spotlight banner ─────────── */

function SpotlightBanner({ spotlight }: { spotlight: Spotlight }) {
  return (
    <section
      className="gallery-spotlight"
      aria-label={`${spotlight.eyebrow}: ${spotlight.meta}`}
    >
      <div className="gallery-spotlight-photo">
        <Image
          src={spotlight.src}
          alt={spotlight.alt}
          fill
          sizes="100vw"
          quality={92}
          loading="lazy"
          className="gallery-spotlight-img"
        />
        <div className="gallery-spotlight-scrim" aria-hidden="true" />
      </div>
      <div className="container-xl gallery-spotlight-inner">
        <p className="gallery-spotlight-eyebrow">{spotlight.eyebrow}</p>
        <blockquote className="gallery-spotlight-quote">
          {spotlight.quote}
        </blockquote>
        <p className="gallery-spotlight-meta">{spotlight.meta}</p>
      </div>
    </section>
  );
}

/* ─────────── Through the Years rail ─────────── */

function TimelineSection({
  onOpenPhoto,
}: {
  onOpenPhoto: (p: GalleryPhoto) => void;
}) {
  const handleClick = (i: number) => {
    const t = TIMELINE[i];
    const match = PHOTOS.find((p) => p.src === t.src);
    if (match) {
      onOpenPhoto(match);
    } else {
      onOpenPhoto({
        id: `tl-${t.year}`,
        src: t.src,
        alt: t.alt,
        title: t.caption,
        event: `Through the Years · ${t.yearLabel}`,
        year: t.year,
        category: "brand-moments",
      });
    }
  };

  return (
    <section
      id="through-the-years"
      aria-labelledby="tty-h"
      className="gallery-timeline-section"
    >
      <div className="container-xl">
        <header className="gallery-section-head">
          <p className="gallery-section-eyebrow">Through the Years</p>
          <h2 id="tty-h" className="gallery-section-heading">
            12 years. <em>One league.</em>
          </h2>
          <p className="gallery-section-blurb">
            From a single weekend to a multi-court, multi-city circuit. Drag
            sideways or tab through the years.
          </p>
        </header>
      </div>

      <div className="gallery-timeline-rail-wrap">
        <ol className="gallery-timeline-rail">
          {TIMELINE.map((t, i) => (
            <li key={t.year} className="gallery-timeline-card">
              <button
                type="button"
                className="gallery-timeline-btn"
                onClick={() => handleClick(i)}
                aria-label={`Open ${t.yearLabel}, ${t.year}: ${t.caption}`}
              >
                <span className="gallery-timeline-imgwrap">
                  <Image
                    src={t.src}
                    alt={t.alt}
                    fill
                    sizes="(max-width: 768px) 70vw, 360px"
                    quality={88}
                    loading="lazy"
                    className="gallery-timeline-img"
                  />
                </span>
                <span className="gallery-timeline-meta">
                  <span className="gallery-timeline-year">{t.year}</span>
                  <span className="gallery-timeline-label">{t.yearLabel}</span>
                  <span className="gallery-timeline-caption">{t.caption}</span>
                </span>
              </button>
            </li>
          ))}
        </ol>
      </div>
    </section>
  );
}
