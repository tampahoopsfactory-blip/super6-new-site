# CLAUDE.md / .cursorrules — Super 6 Website Project

Project context for Claude Code and Cursor. Both files (`CLAUDE.md` and
`.cursorrules`) are kept identical so AI agents behave the same way
regardless of which tool TK is using.

---

## Owner

Timothy Keeley (TK) — tk@thesuper6.com — Super6 Series LLC / Super6 Inc.

---

## Project

`super6-website/` — the public website for Super 6 youth basketball
tournaments. Production target: **www.thesuper6.com**.

**Stack:**
- Next.js 16.1.6 (App Router, Turbopack)
- React 19.2.3
- TypeScript 5
- Tailwind v4 (utility tokens)
- ESLint 9 / `eslint-config-next`
- Stripe (registration checkout)
- `react-markdown` (FAQ + content pages)
- `lucide-react` (icons)

---

## TK communication preferences

- **Direct, efficient. No filler. No emojis.**
- Answer first; explanation only if asked.
- No bullet points unless content is genuinely list-form. Use prose.
- Copy-paste-ready commands and code, always.
- If something is ambiguous, ask **all** clarifying questions upfront — never mid-task.
- Complete tasks end to end. Do not stop to ask mid-process.
- Flag legal / financial / operational risks proactively without being asked.
- Connect dots across businesses. If matter A affects matter B, say so immediately.
- Challenge assumptions when it matters. Be a strategist, not a yes-man.

---

## Engineering rules (non-negotiable)

### Performance & UX
- **Mobile-first.** Design for 375px viewport before desktop.
- **Lighthouse target: 95+** across Performance, Accessibility, Best Practices, SEO.
- Static pages must prerender. No client-side data fetching unless required.
- Hard-reload (Cmd-Shift-R) frequently during dev — Turbopack CSS cache goes stale.

### Design system (the FAQ page is the gold standard)
- **Match `/faq` exactly** for any new editorial page. Reference doc:
  https://docs.google.com/document/d/1mvdrkVibsUYIZ7OPQ1i6LocAb-JYq4-A9IGramTl108/edit
- Pull all colors, fonts, spacing from `src/app/globals.css` design tokens.
  **Never invent new tokens.**
- Source Serif 4 for display, Inter for body — these are the only fonts.
- Orange `#F35422` is the primary accent. BKS gold `#C9A24B` only on
  Big Kelly Security treatments.
- All photos use `--photo-grade` filter.

### `/ai-events` frozen top hero — **TK explicit approval required**

Unless **TK explicitly asks** for a change, treat this region as **locked**:

- **`super6-website/src/app/ai-events/page.tsx`** — the entire first
  `<section className="rules-hero ai-events-hero">` … `</section>` (hero photo,
  meta strip, wordmark **AI / ENGINEERING**, headline, checkpoint bullets).
- **`super6-website/src/app/globals.css`** — **every** selector prefixed
  **`.ai-events-hero`** (photo framing through hero bullet markers).

**You may edit freely without asking:** everything **below** that closing hero
`</section>` (process band, `steps`, features grid, tech callout, CTA).

**Public copy below the hero:** stay **patron- and family-benefit** focused — what
happens at check-in and why it matters. **Do not** publish hardware SKUs, vendor
names for infrastructure, cloud/backend mechanics, webhooks/APIs, encryption
internals, or other trade-secret implementation detail unless **TK explicitly
requests** it.

**Cursor rules:**

- `super6-website/.cursor/rules/ai-events-frozen-top.mdc` — freeze scope +
  allowed edits
- `super6-website/.cursor/rules/ai-events-hero-image.mdc` — photo crop specifics

Photo asset remains **`public/media/uploads/ai-events-gate-kiosk.png`** — **never**
`contain` here; **never** inline hero `<Image>` `object-fit` / `object-position`.

### Component patterns
- Pages are server components in `src/app/<route>/page.tsx`.
- Client logic colocates in `_components/` folders inside the route.
- Data files colocate as `<route>-data.ts` and export typed structures.
- Shared layout (`Navigation.tsx`, `Footer.tsx`) is in `src/components/layout/` —
  do **not** modify unless explicitly asked.
- Cross-route modules go in `src/components/modules/` only when reused.
- Default to colocation.

### Accordion / expand-collapse
- Use a real `<button>` with `aria-expanded` + `aria-controls`.
- Animate via JS-measured `max-height` + `ResizeObserver`. **DO NOT** use
  the `grid-template-rows: 0fr → 1fr` trick (fails in Turbopack/Tailwind v4).
  **DO NOT** use `<details>` (browser markers leak).
- Plus icon rotates 45° to × on open.
- Single-open behavior across the page.
- URL hash sync via `history.replaceState`.

### Accessibility (non-negotiable)
- Every trigger is a real `<button>`.
- Section landmarks with `<section aria-labelledby="…">`.
- Focus-visible rings: `2px solid --orange`, 2px offset.
- `prefers-reduced-motion` honored everywhere.
- `aria-hidden="true"` on decorative SVGs and watermark numerals.

### React 19 gotchas
- **Don't call `setState` synchronously inside `useEffect` on mount** —
  triggers `react-hooks/set-state-in-effect` lint error. Defer via
  `setTimeout(80)` or `queueMicrotask`.
- The pattern is documented in `src/app/faq/_components/FAQClient.tsx`.

### Build / lint
- `npm run build` must pass before declaring work done.
- `npx eslint <route folder>` must return 0 errors / 0 warnings on touched files.
- Pre-existing lint errors in unrelated files are out of scope unless
  TK asks to fix them.

---

## Branching

- Branch per priority tier or per major feature.
- Current working branch: `claude/pensive-cray`.
- Default base: `main`.
- Conventional commit messages preferred.
- Never amend commits unless explicitly asked.

---

## File organization

```
super6-website/
├── src/
│   ├── app/
│   │   ├── <route>/
│   │   │   ├── page.tsx              ← server component
│   │   │   ├── <route>-data.ts       ← typed data + helpers
│   │   │   └── _components/          ← client components for this route
│   │   ├── globals.css               ← design tokens + component CSS
│   │   └── layout.tsx
│   ├── components/
│   │   ├── layout/                   ← Navigation, Footer (don't touch)
│   │   ├── modules/                  ← cross-route reusable
│   │   ├── ui/                       ← primitives (FadeIn, etc.)
│   │   └── ecommerce/
│   └── data/                         ← shared data (gallery, etc.)
├── public/
│   └── media/
│       ├── hero/
│       ├── gallery/
│       ├── logos/
│       └── uploads/                  ← TK-uploaded photos
└── package.json
```

---

## Critical references

- **`super6-FULL-audit.md`** (project root) — ranked production punch
  list (P0 / P1 / P2). The next unchecked item in priority order is
  always the next thing to work on unless TK explicitly redirects.
- **`HANDOFF.md`** (project root) — current session state, what's done,
  what's next, open questions
- **Design brief Google Doc** — canonical FAQ pattern:
  https://docs.google.com/document/d/1mvdrkVibsUYIZ7OPQ1i6LocAb-JYq4-A9IGramTl108/edit
- **`src/app/faq/`** — the gold-standard implementation. Mimic this.
- **`src/app/globals.css`** — design tokens
- **`super6-website/.cursor/rules/ai-events-frozen-top.mdc`** — `/ai-events`
  top hero + `.ai-events-hero` CSS are frozen unless TK explicitly asks

---

## Things to never do

- Don't edit **`/ai-events`** **frozen top hero** or **`.ai-events-hero`** CSS
  unless **TK explicitly requests** it (`super6-website/.cursor/rules/ai-events-frozen-top.mdc`).
- Don't introduce new fonts.
- Don't hard-code colors.
- Don't add framer-motion or other animation libraries — CSS transitions
  + JS measurement is sufficient.
- Don't use `<details>` for accordions.
- Don't use the `grid-template-rows` animation trick.
- Don't modify the global header/nav, footer, or shared layout components
  unless explicitly asked.
- Don't add emojis to any code, comments, commit messages, or UI strings
  unless TK explicitly requests it.
- Don't push to `main`.
- Don't amend commits.
- Don't bypass git hooks (`--no-verify`).
