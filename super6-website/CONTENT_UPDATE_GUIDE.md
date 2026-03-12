# Super6 Website — How to Update Content

## Quick Reference

### File Structure
```
super6-website/
├── src/
│   ├── app/              ← Pages (routes)
│   │   ├── page.tsx      ← Homepage
│   │   ├── about/        ← About page
│   │   ├── contact/      ← Contact page
│   │   ├── register/     ← Registration + pricing
│   │   ├── locations/    ← Location index + detail pages
│   │   └── api/          ← API routes (Stripe checkout)
│   ├── components/
│   │   ├── layout/       ← Header, Footer
│   │   └── modules/      ← Reusable page sections
│   ├── data/
│   │   └── site.ts       ← ALL content data (edit this most)
│   └── lib/
│       └── utils.ts      ← Utility functions
├── public/
│   └── media/            ← All images and videos
│       ├── logos/
│       ├── hero/
│       ├── lifestyle/
│       ├── locations/
│       ├── videos/
│       └── misc/
├── assets-manifest.json  ← Media inventory
└── MEDIA_GAPS.md         ← Missing media list
```

---

## Common Tasks

### 1. Update Site Info (Phone, Email, Social Links)
Edit `src/data/site.ts` → `siteConfig` object:
```ts
export const siteConfig = {
  phone: "(407) 906-4563",    // ← change here
  email: "info@thesuper6.com", // ← change here
  social: { ... },             // ← social URLs here
};
```

### 2. Add/Edit a Location
Edit `src/data/site.ts` → `locations` array. Add a new object:
```ts
{
  slug: "miami",           // URL slug: /locations/miami
  name: "Super6 Miami",
  city: "Miami",
  state: "FL",
  address: "123 Main St, Miami, FL 33101",
  phone: "(305) 555-0100",
  comingSoon: false,       // true = shows "Coming Soon" badge
  image: "/media/locations/miami-venue.jpg",
  description: "Description here...",
}
```
The location detail page is auto-generated from this data.

### 3. Update Registration Pricing
Edit `src/data/site.ts` → `registrationTiers` array:
```ts
{
  id: "single-tournament",
  name: "Single Tournament",
  price: 350,
  priceLabel: "$350",
  features: ["Feature 1", "Feature 2", ...],
}
```

### 4. Add/Replace Photos
1. Drop new images into the appropriate `public/media/` subfolder
2. Reference them in `src/data/site.ts` or directly in page components
3. Use the pattern: `/media/subfolder/filename.jpg`

### 5. Add/Replace Videos
1. Drop MP4 files into `public/media/videos/`
2. Generate a poster image: `ffmpeg -i video.mp4 -vframes 1 -q:v 2 video-poster.jpg`
3. Reference in components with `<video>` tag using `poster` attribute

### 6. Edit Navigation
Edit `src/data/site.ts` → `navigation` array:
```ts
{ label: "New Page", href: "/new-page" }
// Add highlight: true for the CTA button style
```

### 7. Edit Hero Tiles (Homepage)
Edit `src/data/site.ts` → `heroTiles` array. Each tile has:
- `type`: "image" or "video"
- `src`: path to media
- `headline`, `copy`, `cta`: display content

### 8. Edit Story Cards (Homepage)
Edit `src/data/site.ts` → `storyCards` array.

---

## Enabling Stripe Payments

1. Create a Stripe account at https://stripe.com
2. Copy `.env.example` to `.env.local`
3. Fill in your Stripe keys:
   ```
   STRIPE_SECRET_KEY=sk_live_...
   NEXT_PUBLIC_STRIPE_PUBLISHABLE_KEY=pk_live_...
   ```
4. Uncomment the Stripe code in `src/app/api/checkout/route.ts`
5. Update the registration form submit handler in `src/app/register/page.tsx` to call the API

---

## Deploying to Vercel

1. Push to GitHub
2. Connect repo at https://vercel.com/new
3. Set environment variables (Stripe keys, etc.)
4. Deploy — Vercel auto-detects Next.js

---

## Running Locally

```bash
cd super6-website
npm install
npm run dev      # → http://localhost:3000
npm run build    # Production build
npm run start    # Serve production build
```
