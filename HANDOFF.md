# HANDOFF — Super 6 Website

End-of-session handoff from Claude Code to Cursor.

---

## Status as of 2026-05-04 16:50 EDT

- **Current branch:** `claude/pensive-cray`
- **Worktree path:** `/Users/timothykeeley/ACCESS CONTROL PROJECT BOTH AP/Paradym EventShieldPro/DS-F8881 Documents/.claude/worktrees/pensive-cray`
- **Project root inside worktree:** `super6-website/`
- **Last commit:** `6276cc9  WIP: end of Claude Code session — handoff to Cursor`
- **Remote:** `https://github.com/tampahoopsfactory-blip/SUPER6-WEBSITE.git` — pushed to `origin/claude/pensive-cray`
- **Build status (`npm run build`):** PASSING ✓ — compiled in ~2s, 30/30 static pages prerendered
- **Lint status (`npm run lint`):** 8 problems (4 errors, 4 warnings) — **all preexisting** in files NOT touched this session (`Navigation.tsx`, `coaches/page.tsx`, `shop/page.tsx`, `sponsors/page.tsx`, `api/checkout/route.ts`). Files touched this session lint clean.

---

## What's done

This session shipped a major editorial redesign of the FAQ page and the homepage Experience section, plus content updates and visual polish across the site.

### FAQ page rebuild — production-grade (`/faq`)
- New file: `super6-website/src/app/faq/faq-data.ts` — typed `FaqSection[]` with 8 sections, 52 questions, slugs, icons, pending flags, JSON-LD builder
- New folder: `super6-website/src/app/faq/_components/`
  - `FAQClient.tsx` — top-level state container, search filter, scroll-spy via IntersectionObserver, deep-link expand, single-open accordion
  - `FAQItem.tsx` — accordion item with JS-measured max-height animation (ResizeObserver), Plus icon rotates 45° to × on open, react-markdown for answers
  - `FAQSection.tsx` — section spread with watermark numeral + icon medallion + eyebrow + title + italic description + count chip
  - `FAQSearch.tsx` — search bar with focus halo, ESC clear, aria-live status
  - `SectionNav.tsx` — sticky desktop rail (268px) + mobile chip bar with frosted backdrop
  - `SectionIcon.tsx` — Lucide icon registry mapping data string keys to React components
- Updated: `super6-website/src/app/faq/page.tsx` — server component, editorial split hero, JSON-LD FAQPage schema, two-column shell, footnote, final CTA
- Added FAQ to main nav: `super6-website/src/components/layout/Navigation.tsx`
- New CSS block in `super6-website/src/app/globals.css` (~600 lines): editorial card spread, sticky rail, mobile chip bar, search, accordion cards, markdown rendering, empty state, footnote, final CTA
- Installed `react-markdown@^10.1.0` (added to `package.json`)
- 8 sections: Getting Started · The Super6 App · Schedule & Format · Game Rules · Check-In & Gate · Officials & Coaches · Venues & Logistics · Refunds & Weather

### Homepage Experience section → Security Spread (`StoryCardGrid.tsx`)
- Replaced "Built for the moment that matters" Why Super 6 section with a Big Kelly's BKS Security spread
- File: `super6-website/src/components/modules/StoryCardGrid.tsx`
- Asymmetric editorial layout: photo column ~58% with watermark "BKS" gold mark, primary portrait + polaroid-style secondary action shot, vertical hairline divider, numbered protocols (01/02/03 in serif orange), partner signature card with hand-drawn SVG shield
- Game Rules CTA pulled out as a full-width dark editorial banner below the spread (`--ink` background, 4px orange top edge, soft orange radial glow)
- Section background uses `--cream-warm`
- Photos extracted from session: `public/media/uploads/bks-suit.jpg`, `public/media/uploads/bks-bag-check.jpg`

### Homepage hero rewrite (`HeroTileGrid.tsx`)
- Eyebrow: "Florida · Georgia · Est. 2014" → "Florida · Georgia · Since 2014"
- Headline: "SUPER6 where Champions are made!!!" → "SUPER6. Where champions are made."
- New italic kicker: *"The South's standard for youth basketball."*
- Sub paragraph rewritten to flow as a single sentence with em-dash + Oxford comma list ("from the first tip to the last whistle")
- New small-caps closer tag with hairline rule: "Boys and girls · 3rd–12th grade · Every event, every weekend"
- New CSS classes: `.editorial-kicker`, `.editorial-sub-tag`

### Other changes already committed earlier in session
- `ServiceGrid.tsx` (Impact Strip) — Super 6 diamond mark anchored to left of stats
- Officials/Hiring Officials page rebuilt
- Contact page editorial Direct Line format
- Rules page Editorial Rule Book hero
- Game Rules CTA card on Experience section
- No-cash payment notice with branded SVG payment-method logos
- Uniform Compliance officials-monitoring notice
- Transparent Super 6 logo across the site (no white background)
- "Home" as first nav item

### Content / data updates
- Q1 (cost-to-enter): `$50 security & officials fee` → `$25`
- Q22 (jerseys-printing): `burn your $50 fee at the door` → `$25`
- Q2 (how-to-register): converted ordered list (`1. 2. 3.`) to plain bullets

### Reference docs created
- Google Doc: "Super 6 — Editorial Page Design Brief (FAQ Pattern)"
  - URL: https://docs.google.com/document/d/1mvdrkVibsUYIZ7OPQ1i6LocAb-JYq4-A9IGramTl108/edit
  - Captures all 18 design rules, tokens, patterns, accessibility requirements, file organization, and "things to avoid" for replicating the FAQ page treatment elsewhere

---

## What's in flight

Nothing half-finished. Everything committed compiles and runs. Items listed below are deferred work or known stale state.

### Pending TK confirmation (marked `pending: true` in `faq-data.ts`)
The FAQ data file flags 14 items where I drafted plausible policy copy that needs TK to confirm or rewrite. These render with an orange "TODO" pill in the UI:

1. `app-login-issues` — login troubleshooting flow
2. `push-notifications` — required, exact iOS/Android paths
3. `ball-size` — confirmed division-by-division ball sizes
4. `shot-clock` — confirm: no shot clock at any division?
5. `mercy-rule` — the 30-pt running-clock continuation policy I drafted
6. `biometric-entry` — confirm facial recognition use + copy
7. `wristbands` — confirm tampering policy
8. `reentry` — confirm same-day re-entry allowed
9. `coach-credentials` — background check / certification details
10. `ejection-rules` — next-game suspension + 2nd-ejection policy
11. `parking` — confirm all venues have parking
12. `food-policy` — confirm outside-food rule + sealed water exception
13. `weather-delays` — confirm "no refunds for weather, played-game-counts" policy
14. `dispute-resolution` — confirm escalation path is the Contact form

### Untouched in scope but probably stale
- Other homepage sections still use placeholder copy (Programs, Alumni, Testimonials, Pricing, etc.)
- Stub pages exist but minimal: `/players`, `/careers`, `/press`, `/shop`, `/programs/college-pipeline`, `/programs/camps`, `/programs/showcase`, `/programs/training`
- `Navigation.tsx` lint error is preexisting and was flagged but not fixed (out of scope for this session)

### Known broken state
- None. Build is green. `/faq` deep-links work. Homepage renders correctly.

---

## What's next

`super6-FULL-audit.md` was rebuilt at the worktree root on 2026-05-04 by scanning the codebase against the FAQ gold standard. It is the single source of truth for prioritized work. Pull from it in priority order: P0 first, then P1, then P2.

**Closed in this Cursor session (2026-05-04, after handoff):**
- P0-01 — Mega-menu 404s. Repointed to anchored sections on `/programs` that already render (`#youth`, `#travel`, `#school`, `#elite`).
- P0-03 — Sitemap rebuilt to include all 16 indexable public routes.
- P0-04 — `Navigation.tsx` `set-state-in-effect` lint error fixed via `setTimeout` defer.
- P1-04 — Lint debt cleared (0 errors, 3 warnings — all auto-resolve with P0-02).
- P0-05 (partial) — Pruned stub pages from mega-menu and `noindex`'d their metadata.
- P1-02 (`/rules` leg) — Restructured /rules under the FAQ pattern: typed rules-data.ts (7 sections, 39 rules), colocated _components/ adapter (RulesClient/Item/Section/SectionNav/SectionIcon/Search), reuses faq-* CSS classes. Single-open accordion + scroll-spy rail + deep-link expand + ESC-clear search all live.

**Open P0 items:**
1. **P0-02** — `/api/checkout` is still a Stripe mock. `.env.example` now documents the full env surface (webhook secret, price IDs, backing-store options) but actual wiring waits on TK to pick a backing store (Supabase / Airtable / Sheets / email-only).
2. **P0-05 (final leg)** — Per-route decision on `/players`, `/press`, `/careers`, `/shop`: build real content, redirect to siblings, or delete.

See `super6-FULL-audit.md` for full P0/P1/P2 ranking, files, acceptance criteria, and the consolidated TK-decision block at the bottom.

---

## How to resume

### Start the dev server
```bash
cd "/Users/timothykeeley/ACCESS CONTROL PROJECT BOTH AP/Paradym EventShieldPro/DS-F8881 Documents/.claude/worktrees/pensive-cray/super6-website"
npm install
npm run dev
```
Dev server runs at http://localhost:3000 — Turbopack-powered. **Hard-reload (Cmd-Shift-R) frequently** to bust the Turbopack CSS cache, especially after editing `globals.css`.

### Commands you'll need
- `npm run build` — full production build (Turbopack)
- `npm run lint` — ESLint
- `npx eslint <path>` — lint a specific file or folder
- `git log --oneline -20` — recent commits

### Files to read first (in order)
1. **This file** — `HANDOFF.md` (you are here)
2. **Design brief Google Doc** — https://docs.google.com/document/d/1mvdrkVibsUYIZ7OPQ1i6LocAb-JYq4-A9IGramTl108/edit (the canonical pattern reference)
3. **`CLAUDE.md`** in project root (TK's preferences)
4. **`super6-website/src/app/faq/`** — the gold-standard implementation. Mimic this when building new pages.
5. **`super6-website/src/app/globals.css`** — design tokens and component CSS
6. **`super6-website/README.md`** — original project README

### Env / setup
- Node 20+, npm 10+
- No env vars required for dev. Stripe keys are required only for `/api/checkout` (the registration flow), and the existing `.env.local` (if any) is gitignored.
- `react-markdown` was added this session — `npm install` will pick it up from `package.json`.

---

## Open questions for TK

The audit (`super6-FULL-audit.md`) consolidates these into a single decision block at its bottom. The high-leverage answers needed before the next session can fully execute:

1. **P0-01** — Build dedicated `/programs/<slug>` pages or collapse to anchored sections?
2. **P0-02** — Registration backing store (Supabase / Airtable / Google Sheets webhook / email-only) and confirm pricing ($99 single / $899 season).
3. **P0-05** — Of `/players`, `/press`, `/careers`, `/shop`, which are launch-required vs. removable from nav?
4. **P1-01** — 14 FAQ pending items: dictate answers directly or have the agent draft tighter copy for review?
5. **P1-03** — Real testimonial and alumni names, or remove those sections from the homepage stack until data exists?
6. **Pre-existing question** — BKS Security spread: keep as the replacement of "Why Super 6" only, or also bring back as its own dedicated band?
7. **Pre-existing question** — Hero photo `celtics-super6.jpg`: keep or swap for newer 2026 imagery?
