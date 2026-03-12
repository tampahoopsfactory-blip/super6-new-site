import Image from "next/image";

/* ─── Photo Gallery — Magazine bento grid
   Nike: full-bleed photography, tight gaps, no chrome.
   Uses real game photos from the gallery. ─── */

const photos = [
  { src: "/media/gallery/G1_01_Dunk_Action.jpg", alt: "Slam dunk action shot" },
  { src: "/media/gallery/G1_30_Fast_Break.jpg", alt: "Fast break play" },
  { src: "/media/gallery/G2_25_Award_Presentation.jpg", alt: "Award ceremony" },
  { src: "/media/gallery/G1_08_Team_Huddle_Celebration.jpg", alt: "Team celebration" },
  { src: "/media/gallery/G1_27_Contested_Drive.jpg", alt: "Contested drive to basket" },
];

export default function PhotoGallery() {
  return (
    <section className="section" aria-label="Gallery" style={{ padding: "40px 0", background: "var(--black)" }}>
      <div className="container-xl" style={{ padding: 0, maxWidth: "100%" }}>
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
              />
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
