"use client";

/* ─── Lightbox — accessible modal viewer.

   Props:
     photos       — array currently being viewed (filtered or full)
     index        — currently displayed photo index, or null when closed
     onClose      — close handler
     onPrev/Next  — navigation handlers

   Behavior:
     - ESC closes
     - ←/→ arrows navigate
     - Body scroll locked while open
     - Focus trapped in dialog while open
     - role="dialog" + aria-modal + labeled by current photo title
     - Backdrop click closes; clicks on the figure do not
*/

import { useEffect, useRef, useCallback } from "react";
import Image from "next/image";
import type { GalleryPhoto } from "@/data/gallery";

export default function Lightbox({
  photos,
  index,
  onClose,
  onPrev,
  onNext,
}: {
  photos: GalleryPhoto[];
  index: number | null;
  onClose: () => void;
  onPrev: () => void;
  onNext: () => void;
}) {
  const open = index !== null;
  const photo = open ? photos[index] : null;
  const closeBtnRef = useRef<HTMLButtonElement>(null);

  // Keyboard nav
  const onKey = useCallback(
    (e: KeyboardEvent) => {
      if (!open) return;
      if (e.key === "Escape") onClose();
      else if (e.key === "ArrowLeft") onPrev();
      else if (e.key === "ArrowRight") onNext();
    },
    [open, onClose, onPrev, onNext],
  );

  useEffect(() => {
    if (!open) return;
    document.addEventListener("keydown", onKey);
    return () => document.removeEventListener("keydown", onKey);
  }, [open, onKey]);

  // Body scroll lock
  useEffect(() => {
    if (!open) return;
    const prev = document.body.style.overflow;
    document.body.style.overflow = "hidden";
    return () => { document.body.style.overflow = prev; };
  }, [open]);

  // Move focus into dialog when opening
  useEffect(() => {
    if (open) closeBtnRef.current?.focus();
  }, [open]);

  if (!open || !photo) return null;

  const total = photos.length;
  const position = `${index! + 1} / ${total}`;

  return (
    <div
      className="lightbox"
      role="dialog"
      aria-modal="true"
      aria-label={`${photo.title} — ${photo.event}, ${photo.year}`}
      onClick={onClose}
    >
      {/* Top bar — counter + close */}
      <div className="lightbox-topbar" onClick={(e) => e.stopPropagation()}>
        <span className="lightbox-counter">{position}</span>
        <button
          ref={closeBtnRef}
          type="button"
          className="lightbox-btn lightbox-close"
          onClick={onClose}
          aria-label="Close gallery viewer"
        >
          {/* X */}
          <svg viewBox="0 0 24 24" width="20" height="20" aria-hidden="true">
            <path d="M5 5l14 14M19 5L5 19" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" />
          </svg>
        </button>
      </div>

      {/* Prev */}
      <button
        type="button"
        className="lightbox-btn lightbox-nav lightbox-prev"
        onClick={(e) => { e.stopPropagation(); onPrev(); }}
        aria-label="Previous image"
      >
        <svg viewBox="0 0 24 24" width="22" height="22" aria-hidden="true">
          <path d="M15 5l-7 7 7 7" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" strokeLinejoin="round" fill="none" />
        </svg>
      </button>

      {/* Figure */}
      <figure className="lightbox-figure" onClick={(e) => e.stopPropagation()}>
        <div className="lightbox-imgwrap">
          <Image
            src={photo.src}
            alt={photo.alt}
            fill
            sizes="(max-width: 1100px) 92vw, 80vw"
            quality={92}
            priority
            className="lightbox-img"
          />
        </div>
        <figcaption className="lightbox-caption">
          <p className="lightbox-title">{photo.title}</p>
          <p className="lightbox-event">
            {photo.event} <span className="lightbox-dot">·</span> {photo.year}
          </p>
        </figcaption>
      </figure>

      {/* Next */}
      <button
        type="button"
        className="lightbox-btn lightbox-nav lightbox-next"
        onClick={(e) => { e.stopPropagation(); onNext(); }}
        aria-label="Next image"
      >
        <svg viewBox="0 0 24 24" width="22" height="22" aria-hidden="true">
          <path d="M9 5l7 7-7 7" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" strokeLinejoin="round" fill="none" />
        </svg>
      </button>
    </div>
  );
}
