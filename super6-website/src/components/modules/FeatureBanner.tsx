import Image from "next/image";

/* ─── Photo Gallery — Editorial bento grid
   Full-bleed photography on cream surface, no chrome.
   The first tile spans both rows for a magazine-feature feel. */

const photos = [
  { src: "/media/curated/01-flagship-coach-pointer.jpg", alt: "Coach mentoring a player on the sideline" },
  { src: "/media/curated/05-crowd-eruption.jpg", alt: "Crowd reacting to a key moment in the stands" },
  { src: "/media/curated/09-trophy-raise.jpg", alt: "Team raising a championship trophy together" },
  { src: "/media/curated/15-kids-whiteboard.jpg", alt: "Young players studying a play diagram" },
  { src: "/media/curated/03-drive-isolation.jpg", alt: "Player driving against a defender in front of a packed crowd" },
];

export default function PhotoGallery() {
  return (
    <section className="section-paper" aria-label="Gallery" style={{ padding: "0" }}>
      <div className="gallery-grid">
        {photos.map((photo, i) => (
          <div key={photo.src} className="gallery-item">
            <Image
              src={photo.src}
              alt={photo.alt}
              fill
              sizes={i === 0 ? "(max-width: 768px) 100vw, 34vw" : "(max-width: 768px) 100vw, 33vw"}
              className="object-cover"
              priority={i === 0}
              quality={88}
            />
          </div>
        ))}
      </div>
    </section>
  );
}
