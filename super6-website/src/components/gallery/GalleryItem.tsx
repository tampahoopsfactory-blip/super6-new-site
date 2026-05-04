"use client";

/* ─── GalleryItem — single tile.
   Renders a clickable button wrapping a Next/Image. Caption fades up
   on hover. All tiles render at a uniform 4:5 ratio with no
   2-column spans — fully consistent grid. The `aspect` and `featured`
   fields on the photo record are intentionally ignored here. */

import Image from "next/image";
import type { GalleryPhoto } from "@/data/gallery";

export default function GalleryItem({
  photo,
  index,
  onOpen,
  priority = false,
}: {
  photo: GalleryPhoto;
  index: number;
  onOpen: (index: number) => void;
  priority?: boolean;
}) {
  return (
    <button
      type="button"
      onClick={() => onOpen(index)}
      className="gallery-tile"
      aria-label={`Open ${photo.title}, ${photo.event}, ${photo.year}`}
    >
      <Image
        src={photo.src}
        alt={photo.alt}
        fill
        sizes="(max-width: 640px) 50vw, (max-width: 1100px) 33vw, 25vw"
        quality={88}
        loading={priority ? "eager" : "lazy"}
        priority={priority}
        className="gallery-tile-img"
      />
      <div className="gallery-tile-overlay" aria-hidden="true">
        <div className="gallery-tile-meta">
          <p className="gallery-tile-title">{photo.title}</p>
          <p className="gallery-tile-event">
            {photo.event} <span className="gallery-tile-dot">·</span> {photo.year}
          </p>
        </div>
      </div>
    </button>
  );
}
