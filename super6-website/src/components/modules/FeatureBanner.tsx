import Image from "next/image";

/* ─── Editorial Spread — two-photo horizontal split.
   Side-by-side cinematic moments with a slim gap.
   Both slots use placeholder images — swap in real photos
   when TK provides them. */

const PHOTO_SLOTS = [
  {
    src: "/media/uploads/showout-9-drive.jpg",
    alt: "Showout All-Stars #9 driving past defender during Super6 tournament action",
    objectPosition: "center 30%",
    placeholder: false,
  },
  {
    src: "/media/uploads/orlando-jazz-drive.jpg",
    alt: "Orlando Jazz #1 driving past E1T1 defenders during Super6 tournament action",
    objectPosition: "center 35%",
    placeholder: false,
  },
] as const;

export default function PhotoGallery() {
  return (
    <section
      className="editorial-duo"
      aria-label="Editorial moment"
    >
      <div className="editorial-duo-grid">
        {PHOTO_SLOTS.map((slot, i) => (
          <div key={i} className="editorial-duo-frame">
            <Image
              src={slot.src}
              alt={slot.alt}
              fill
              quality={92}
              sizes="(max-width: 768px) 100vw, 50vw"
              style={{
                objectFit: "cover",
                objectPosition: slot.objectPosition,
              }}
            />
            {slot.placeholder && (
              <span className="editorial-duo-placeholder-tag" aria-hidden="true">
                Photo {i + 1} — Placeholder
              </span>
            )}
          </div>
        ))}
      </div>
    </section>
  );
}
