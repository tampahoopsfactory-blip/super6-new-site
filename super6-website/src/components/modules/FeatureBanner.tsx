import Image from "next/image";

/* ─── Editorial Spread — single full-bleed cinematic moment.
   Replaces the 5-photo bento with one magazine-cover image.
   Photo: the flagship coach-mentor moment. Strongest single
   image in the library. */

export default function PhotoGallery() {
  return (
    <section
      aria-label="Editorial moment"
      style={{ background: "var(--cream)", padding: "96px 0" }}
    >
      <div
        style={{
          position: "relative",
          width: "100%",
          aspectRatio: "21 / 9",
          minHeight: 480,
          overflow: "hidden",
          background: "var(--ink)",
        }}
      >
        <Image
          src="/media/uploads/celtics-super6.jpg"
          alt="Super 6 Series tournament — Celtics player driving past defender with Super 6 banner in background"
          fill
          quality={92}
          sizes="100vw"
          style={{ objectFit: "cover", objectPosition: "center center" }}
        />
      </div>
    </section>
  );
}
