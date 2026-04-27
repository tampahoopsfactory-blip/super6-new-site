import Image from "next/image";

/* ─── Editorial Spread — single full-bleed cinematic moment.
   Replaces the 5-photo bento with one magazine-cover image.
   Photo: the flagship coach-mentor moment. Strongest single
   image in the library. */

export default function PhotoGallery() {
  return (
    <section
      aria-label="Editorial moment"
      style={{ background: "var(--cream)", padding: 0 }}
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
          src="/media/curated/15-kids-whiteboard.jpg"
          alt="Young Super 6 players studying a play diagram together"
          fill
          quality={92}
          sizes="100vw"
          style={{ objectFit: "cover", objectPosition: "center 35%" }}
        />
      </div>
    </section>
  );
}
