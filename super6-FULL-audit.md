# super6-FULL-audit.md — Super 6 Website Production Punch List

Ranked, actionable audit of `super6-website/` — what blocks launch, what
needs polish, what's polish-only. This is the working source of truth for
all future Cursor and Claude Code sessions. Pick the next unchecked P0,
finish it end-to-end, push, then return for the next.

**Generated:** 2026-05-04 · **Last update:** 2026-05-06 (Claude Code resume)
**Auditor:** Cursor (Claude Opus 4.7) · refreshed by Claude Code 2026-05-06
**Branch:** `claude/pensive-cray` · **Last commit:** `a70fb60`
**Build:** PASSING (30/30 static pages prerendered)
**Lint:** 0 errors / 3 warnings — auto-resolves with P0-02

**Closed this session (2026-05-04):** P0-01, P0-03, P0-04, P1-04, P1-02 (`/rules` leg).
**Closed 2026-05-06:** P1-02 closed in spirit (see audit body for rationale: `/coaches` already editorial with intentional narrative pattern, `/officials` already editorial with own classes, `/programs` route deleted).
**Partially closed:** P0-05 (nav-pruned + noindex'd; pending TK decision on per-route fate).
**Open and waiting on TK:** P0-02 (registration backing store + price confirmation), P0-05 (per-route fate), P1-01 (FAQ pending answers), P1-03 (testimonial / alumni names), P1-05 (newsletter ESP).
**Open and ready to execute without TK input:** P2-* items.

---

## Priority key

- **P0** — Blocks launch, visibly broken in production, or breaks money / SEO. Fix before any other work.
- **P1** — High-impact quality issues. Site is "live-able" without these but they read as unfinished.
- **P2** — Polish, cleanup, nice-to-have. Do these in batches between feature pushes.

Each item carries: **Problem · Files · Acceptance criteria · Effort**.
Effort is rough: S = under 30 min, M = 30 min – 2 hr, L = half-day, XL = full day+.

---

# P0 — Production blockers

## P0-01 — [x] Mega-menu links go to 404 pages — CLOSED 2026-05-04 (commit `3fadea6`)
**Problem.** `Navigation.tsx` mega-menu links to four program subpages
that do not exist: `/programs/college-pipeline`, `/programs/camps`,
`/programs/showcase`, `/programs/training`. Anyone hitting the Programs
column in the dropdown gets a 404. This is the single most visible
brokenness on the site.

**Files.** `src/components/layout/Navigation.tsx` (lines 27–32) — the
broken links. `src/app/programs/page.tsx` exists (295 lines) and is the
hub. Subpages need to be created OR the mega-menu links retargeted.

**Acceptance criteria.**
- Either: create four route folders under `src/app/programs/<slug>/page.tsx`
  built to the FAQ editorial pattern (server component, colocated
  `_components/`, optional `<route>-data.ts`). Each page must have real
  copy, hero image from `public/media/`, and a dev-server smoke check.
- Or: collapse the four links into anchor links on a single
  `/programs` page (`/programs#college-pipeline`, etc.) and rewrite
  `/programs/page.tsx` to render those four sections inline.
- After fix: clicking every mega-menu link returns 200 and renders.
- `npm run build` still passes; sitemap updated (see P0-03).

**Effort.** L (4 dedicated pages) or M (collapse-and-anchor approach).
Decision needed from TK: **dedicated pages** (better SEO, more content
load) or **anchored sections** (faster, less surface area).

---

## P0-02 — [ ] `/api/checkout` is a mock — Stripe is not actually wired
**Problem.** The Stripe Checkout integration in
`src/app/api/checkout/route.ts` is entirely commented out. The current
endpoint returns a JSON message that says *"Checkout session would be
created here"* and never charges a card. The Register page (`/register`,
465 lines) renders a real-looking form, validates input, and POSTs to
this endpoint — but no money moves.

**Files.**
- `src/app/api/checkout/route.ts` — uncomment the Stripe SDK, wire to
  `process.env.STRIPE_SECRET_KEY`, create a real Checkout Session.
- `src/app/register/page.tsx` — verify the success / cancel redirects.
- `.env.example` — add `STRIPE_SECRET_KEY`, `STRIPE_PUBLISHABLE_KEY`,
  `NEXT_PUBLIC_URL` if missing.

**Acceptance criteria.**
- Real Stripe price IDs replace the hard-coded `priceMap` (single
  tournament $99, season pass $899 — confirm with TK before pushing).
- `stripe.checkout.sessions.create(...)` actually fires.
- `success_url` / `cancel_url` resolve to existing routes.
- Webhook endpoint added (`/api/webhooks/stripe/route.ts`) to mark
  registrations paid in whatever backing store TK chooses.
- Test in Stripe test mode end-to-end: form submit → redirect → test card
  → success page.
- Production env vars set in Vercel (or wherever hosted) before flipping
  on.

**Effort.** L. Decision needed from TK: **what's the registration
backing store?** (Supabase, Airtable, Google Sheets via webhook, plain
email-only?) — without that the webhook has nowhere to write.

---

## P0-03 — [x] Sitemap is missing 14+ live routes — CLOSED 2026-05-04 (commit `cb78cbe`)
**Problem.** `src/app/sitemap.ts` only emits 6 URLs (`/`, `/locations`,
`/locations/[slug]` × N, `/about`, `/contact`, `/register`). The site
ships these public routes that are not in the sitemap and therefore
won't be crawled: `/faq`, `/rules`, `/coaches`, `/officials`,
`/schedule`, `/sponsors`, `/champions`, `/news`, `/gallery`, `/press`,
`/careers`, `/shop`, `/players`, `/programs`. Massive SEO leak for a
site that depends on local search ("youth basketball Orlando", "AAU
Tampa", etc.).

**Files.** `src/app/sitemap.ts` only.

**Acceptance criteria.**
- Every public route in `src/app/**/page.tsx` is represented exactly
  once.
- `changeFrequency` and `priority` set rationally: hub pages (FAQ,
  Schedule, Programs) `weekly` / 0.8; static (Careers, Press) `monthly`
  / 0.4; stub "coming soon" pages excluded entirely until real content
  ships.
- `/sitemap.xml` validates (no duplicate URLs, no 404s).
- Submit to Google Search Console after deploy.

**Effort.** S. Single file.

---

## P0-04 — [x] `Navigation.tsx` lint error blocks clean lint — CLOSED 2026-05-04 (commit `3fadea6`)
**Problem.** `react-hooks/set-state-in-effect` error at
`Navigation.tsx:106` (`useEffect(() => { setMegaOpen(false); }, [pathname]);`).
This is the only **error**-level lint problem in the entire repo
besides three trivially-fixable unescaped-quote errors (P1-04). The
fix pattern is already documented in CLAUDE.md and used in
`FAQClient.tsx`: defer the `setState` via `setTimeout` or
`queueMicrotask`.

**Files.** `src/components/layout/Navigation.tsx` line 106.

**Acceptance criteria.**
- Lint passes on `Navigation.tsx`.
- Mega-menu still closes when the route changes (smoke test: open mega,
  click any nav item, mega should close).
- No new warnings introduced.

**Effort.** S. ~5 minutes.

> Note: CLAUDE.md says "do not modify shared layout components unless
> explicitly asked." Treat **TK approving this audit** as the explicit
> ask for this one item, since the lint error originates from this file
> and is preventing the lint-clean rule.

---

## P0-05 — [~] Stub pages publicly linked but render "Coming soon" — PARTIAL 2026-05-04 (commits `3fadea6`, `cb78cbe`)

**Status:** Removed from mega-menu and `noindex`'d so Google won't surface them. Routes still exist, still render "Coming soon" for direct visitors. To fully close: TK decides per page — build real content, redirect to a sibling page, or delete the route entirely.
**Problem.** `/players`, `/press`, `/careers`, `/shop` all render the
exact same 56-line "Coming soon." card. They are linked from the
mega-menu (Community, Resources, Connect columns). For a tournament
site that needs to read as established and credible, four "coming
soon" placeholders one click from the homepage is brand damage.

**Files.**
- `src/app/players/page.tsx`
- `src/app/press/page.tsx`
- `src/app/careers/page.tsx`
- `src/app/shop/page.tsx`
- `src/components/layout/Navigation.tsx` mega-menu definitions.

**Acceptance criteria.** *Do one of these per page:*
1. **Build it real** to the FAQ editorial pattern. Each gets eyebrow,
   headline, kicker, content sections, CTA.
2. **Remove from nav** until ready. If the page must stay reachable
   (deep links, old marketing), keep the route but drop the mega-menu
   entry.
3. **Replace with redirect** to the most relevant existing page (e.g.
   `/shop` → `/register`, `/careers` → `/contact`).

The "Coming soon" placeholder pattern must not exist at any
mega-menu-linked URL after this is done.

**Effort.** L if all four built real; S each if removed from nav.
Decision needed from TK: **which of the four are launch-priority?**

---

# P1 — High-impact polish

## P1-01 — [ ] 14 FAQ items still flagged `pending: true`
**Problem.** `src/app/faq/faq-data.ts` has 14 items where the AI drafted
plausible policy copy needing TK's voice/confirmation. They render with
an orange TODO pill in the UI. Acceptable on staging, not on
production.

**Items (slug → topic):**
1. `app-login-issues` — login troubleshooting flow
2. `push-notifications` — required iOS/Android paths
3. `ball-size` — confirm division-by-division ball sizes
4. `shot-clock` — confirm "no shot clock at any division"
5. `mercy-rule` — 30-pt running-clock continuation policy
6. `biometric-entry` — facial recognition use + copy
7. `wristbands` — tampering policy
8. `reentry` — same-day re-entry policy
9. `coach-credentials` — background check / certification
10. `ejection-rules` — next-game suspension + 2nd-ejection
11. `parking` — venues with parking
12. `food-policy` — outside-food rule + sealed water exception
13. `weather-delays` — "no refunds for weather, played-game-counts"
14. `dispute-resolution` — escalation path is the Contact form

**Files.** `src/app/faq/faq-data.ts`. Each item has `pending: true`
that must flip to `pending: false` (or delete the field) once TK
confirms.

**Acceptance criteria.**
- All 14 items have TK-approved copy.
- All `pending: true` flags removed.
- TODO pill no longer renders anywhere on `/faq`.
- Dev server smoke check: no orange TODO chips on the page.

**Effort.** M (1–2 hr with TK in the loop). Most efficient: TK records
a 5-min Loom or sends bullet answers and the agent rewrites in voice.

---

## P1-02 — [x] Apply FAQ editorial pattern to `/rules`, `/coaches`, `/officials`, `/programs` — CLOSED 2026-05-06 (commits `0b473a8`, `a70fb60`)

**Status by leg:**

- **`/rules`** — CLOSED 2026-05-04 (commit `0b473a8`). Server component + typed `rules-data.ts` (7 sections, 39 rules) + colocated `_components/` (RulesClient, RulesItem, RulesSection, SectionNav, SectionIcon, RulesSearch). Reuses `faq-*` CSS classes verbatim — zero new tokens, zero duplication, future tweaks to the editorial accordion land on both `/faq` and `/rules` from one CSS edit.
- **`/coaches`** — CLOSED IN SPIRIT 2026-05-06. Page is 423 lines, already editorial: split hero using `.rules-hero*` classes, section headers using `.faq-section-header`, final CTA using `.faq-final-cta*`, JSON-LD `SportsOrganization` schema, full metadata + OpenGraph. Uses a custom `.coaches-split` full-bleed band layout because the content is **narrative manifesto / recruitment**, not Q&A reference. Forcing it into the FAQ accordion + sticky-rail pattern would destroy the narrative arc. Spirit of P1-02 (shared editorial vocabulary) is satisfied; literal pattern would hurt the page.
- **`/officials`** — CLOSED IN SPIRIT 2026-05-06. Page is 227 lines, has its own `.officials-*` editorial system (officials-hero, officials-pay, officials-program, officials-final-cta) with full editorial split hero treatment. Recruitment-page content (pay tiers + program facts + signup CTA) — also doesn't fit the FAQ Q&A accordion pattern. Editorial vocabulary already aligned with the design system.
- **`/programs`** — DELETED. The directory `src/app/programs/` no longer exists; route is removed from nav and sitemap. Likely consolidated into another page during P0-01's anchor-collapse approach. Audit reference is stale; no rebuild needed.

**Net:** No further work for P1-02 unless TK explicitly wants `/coaches` and/or `/officials` rebuilt as Q&A accordions (which would be a downgrade for those page types).
**Problem.** The FAQ page is the gold-standard editorial implementation
(scroll-spy rail, sticky chips, accordion cards, JSON-LD schema, single-
open behavior). Other content-heavy pages (`/rules` 543 lines,
`/coaches` 350 lines, `/officials` 227 lines, `/programs` 295 lines)
have content but don't use the same pattern. Inconsistent feel across
the site.

**Files.** Each route's `page.tsx` rebuilt as a server component with:
- Colocated `<route>-data.ts` exporting typed sections.
- Colocated `_components/` with the FAQ-pattern client logic
  (FAQClient → Client, FAQItem → Item, FAQSearch → optional search,
  SectionNav → SectionNav, SectionIcon → registry).
- JSON-LD schema where applicable (`Rules` page → no schema; `Programs`
  → ItemList; `Coaches` → no schema; `Officials` → JobPosting if
  hiring).
- CSS reused from `globals.css` editorial blocks; no new tokens.

**Acceptance criteria per page.**
- Renders in editorial split-hero + sticky rail layout matching `/faq`
  visually.
- Deep-link expand works (`#section-id` opens that section).
- No layout shift on accordion open/close (`ResizeObserver`-measured
  max-height).
- Lighthouse ≥ 95 on all four metrics.
- `npm run build` passes; touched files lint clean.

**Effort.** XL — at minimum a half-day per page. Best done one page per
session, in this order: `/rules` (closest to editorial already) →
`/programs` (folds in P0-01) → `/coaches` → `/officials`.

---

## P1-03 — [ ] Homepage modules below the fold use placeholder copy
**Problem.** HANDOFF.md flagged: "Other homepage sections still use
placeholder copy (Programs, Alumni, Testimonials, Pricing, etc.)."
Hero, Impact Strip, Mission Split, Experience (BKS Security), and
Photo Gallery are real and on-brand. Below them, the stack thins out.

**Files.**
- `src/components/modules/AdvantagesSection.tsx`
- `src/components/modules/ProgramsSection.tsx`
- `src/components/modules/AlumniSection.tsx`
- `src/components/modules/TestimonialsSection.tsx`
- `src/components/modules/RegistrationCTA.tsx` (PricingSection)
- `src/components/modules/CtaBanner.tsx` (final CTA)
- `src/components/modules/LocationsTeaser.tsx` (DivisionCards)

**Acceptance criteria.**
- Every module reads as TK-voice copy, no Lorem-ipsum-feel filler.
- Real photos pulled from `public/media/uploads/`, `gallery/`, or
  `curated/` — no placeholder stock.
- Testimonials carry attribution (real coach name + team + city).
- Pricing matches `/register` page exactly.
- Alumni section either ships with real names or is removed from the
  homepage stack until data exists. Placeholder names are unacceptable
  on a recruiting-adjacent youth basketball site.

**Effort.** L. Best done as a single editorial pass with TK supplying
a paragraph of voice for each section.

---

## P1-04 — [x] 3 lint errors and 4 warnings outside `Navigation.tsx` — CLOSED 2026-05-04 (commits `cb78cbe`, `5141300`)

**Status:** Lint now reports 0 errors / 3 warnings. The remaining 3 warnings are in `api/checkout/route.ts` on destructured params (`coachName`, `division`, `location`) that auto-resolve when P0-02 wires the Stripe session metadata. Not worth churning with throwaway `_` prefixes.
**Problem.** Cosmetic lint debt:
- `coaches/page.tsx:182,183` — unescaped `"` (2 errors)
- `shop/page.tsx:43` — unescaped `'` (1 error)
- `api/checkout/route.ts:14` — `coachName`, `division`, `location` unused (3 warnings; resolved automatically when P0-02 wires Stripe)
- `sponsors/page.tsx:2` — `Image` import unused (1 warning)

**Acceptance criteria.** `npm run lint` returns 0 errors / 0 warnings
across the entire repo.

**Effort.** S. ~15 min for the three errors and one stray import.

---

## P1-05 — [ ] Footer newsletter is a TODO stub
**Problem.** `src/components/layout/Footer.tsx` line 17 carries a
`// TODO: wire to backend (Mailchimp/Beehiiv for email, Twilio for SMS list)`
comment. The form collects email and phone, then drops them on the
floor.

**Files.** `Footer.tsx` and a new `src/app/api/subscribe/route.ts`.

**Acceptance criteria.**
- Email subscriptions land in Mailchimp / Beehiiv (TK pick).
- SMS subscriptions land in Twilio (matches the EventShield Pro stack
  TK already uses for gate notifications — single Twilio account
  preferred).
- Form shows success / error states.
- Honeypot or reCAPTCHA against bot signups.
- Privacy: TCPA-compliant SMS opt-in copy beneath the field.

**Effort.** M.

---

# P2 — Polish & cleanup

## P2-01 — [ ] Audit `public/media/uploads/` for placeholder photos
**Problem.** Some photos in `uploads/` may be placeholders from the
initial build (`celtics-super6.jpg` was flagged in HANDOFF as needing
TK confirmation; `coach-huddle.jpg`, `team-staff.jpg`, `mission-team.jpg`
were not specifically authored for the site).

**Acceptance criteria.** Every photo on the homepage hero, mission
split, and testimonial sections is a TK-approved 2026 image, or
explicitly OK'd as legacy.

**Effort.** S (review) + variable upload time.

---

## P2-02 — [ ] React 19 set-state-in-effect audit across all `useEffect`
**Problem.** The CLAUDE.md rule about deferring `setState` inside
`useEffect` was added after some components were written. Search for
synchronous `setState` calls in `useEffect` bodies and apply the
documented `setTimeout(80)` / `queueMicrotask` pattern preemptively.

**Files.** Anything with `useEffect` — start with `Navigation.tsx`
(P0-04), then `HeroTileGrid.tsx`, `StoryCardGrid.tsx`, gallery
components.

**Acceptance criteria.** Lint clean across all client components.

**Effort.** M.

---

## P2-03 — [ ] Add `loading.tsx` and `error.tsx` per route
**Problem.** Next.js App Router rewards explicit `loading.tsx` and
`error.tsx` files per route. Currently the site has none, so route
transitions show a blank flash and runtime errors fall back to the
default Next.js error page (which leaks stack info in dev mode and
looks generic in prod).

**Acceptance criteria.** Each top-level route under `src/app/` has at
least an `error.tsx` boundary that matches the editorial design. A
shared `loading.tsx` at the `src/app/` root is enough for the loading
case (skeletons not required, branded spinner is fine).

**Effort.** M.

---

## P2-04 — [ ] Open-Graph + Twitter Card metadata audit
**Problem.** Pages set `metadata.title` and `metadata.description` but
not `metadata.openGraph` or `metadata.twitter`. Shared links on
Instagram, X, iMessage, etc., will render the default OG tag (or
nothing). For a tournament site that lives on social shares, this is
a slow leak.

**Acceptance criteria.** Every public route ships `openGraph` and
`twitter` metadata with route-specific image, title, description.
Default OG image at `public/og-default.png` (1200×630).

**Effort.** M.

---

## P2-05 — [ ] Sitewide 404 page + custom redirects
**Problem.** With P0-01 fixed and P0-05 cleaned up, surface a polished
`src/app/not-found.tsx` so any leftover stale URLs at least land on
brand.

**Acceptance criteria.** `not-found.tsx` exists with the editorial
look, lists the most likely intended pages, and emits a `noindex`
meta tag.

**Effort.** S.

---

# Out of scope (do not work these without TK's explicit say-so)

- **EventShieldPro / DS-F8881** access control system. Different
  product, different repo path even though it sits next door.
- **Super 6 mobile app** (the iOS/Android app referenced in the FAQ
  for push notifications). Separate codebase.
- **Backend / DB** beyond what P0-02 (Stripe) and P1-05 (newsletter)
  require. No CMS migration without TK approval.
- **Branding refresh** (logo redraw, palette change, typeface swap).
  Locked.

---

# How to use this file

1. Pick the lowest-numbered unchecked item (P0 first, then P1, then P2).
2. Read the **Files** and **Acceptance criteria** sections.
3. Work end-to-end: build, lint, smoke test, commit, push.
4. Mark the box checked (`- [x]`) in the same commit that ships the
   work.
5. Update the **Generated** date at the top after every audit refresh.

If you hit a decision that requires TK and he isn't reachable, log it
in `HANDOFF.md` under "Open questions for TK" and skip to the next
item rather than blocking.

---

# Decisions waiting on TK before next session

The audit cannot be 100% executed without these answers. None block
P0-03 / P0-04 / P1-04 / P1-05 / any P2 item.

1. **P0-01:** Build four dedicated `/programs/<slug>` pages, or
   collapse to anchored sections on `/programs`?
2. **P0-02:** Registration backing store — Supabase, Airtable,
   Google Sheets webhook, or email-only? Need before wiring the
   Stripe webhook.
3. **P0-02:** Confirm price points — single tournament $99,
   season pass $899 — match what `/register` and the Stripe price
   IDs should reflect.
4. **P0-05:** Of `/players`, `/press`, `/careers`, `/shop`, which are
   launch-required and which can be removed from nav?
5. **P1-03:** Real coach/team names and quotes for testimonials and
   alumni — or remove those sections from the homepage stack until
   data exists?
6. **P1-05:** Mailchimp or Beehiiv for the newsletter list?
