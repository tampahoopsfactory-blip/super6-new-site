This is the [Next.js](https://nextjs.org) project for **www.thesuper6.com** — the public website for Super 6 youth basketball tournaments.

## Resume Development

Picking this project back up (or continuing in Cursor after a Claude Code session)? Read these in order:

1. **`../super6-FULL-audit.md`** — ranked production punch list (P0 / P1 / P2). Single source of truth for what to work on next.
2. **`../HANDOFF.md`** — current session state: what's done, what's in flight, what's next, open questions for TK.
3. **`../CLAUDE.md`** / **`../.cursorrules`** — TK preferences and engineering rules. Both files are kept identical so the agent behaves the same in Claude Code and Cursor. Do not edit one without mirroring the other.
4. **Design brief Google Doc** — canonical FAQ-pattern reference: <https://docs.google.com/document/d/1mvdrkVibsUYIZ7OPQ1i6LocAb-JYq4-A9IGramTl108/edit>
5. **`src/app/faq/`** — gold-standard editorial implementation. Mimic this structure (server `page.tsx`, colocated `_components/`, typed `<route>-data.ts`) when building new content pages.
6. **`src/app/globals.css`** — design tokens and component CSS. Pull all colors / spacing / fonts from here; never invent new tokens.

### Quick start

```bash
cd "/Users/timothykeeley/ACCESS CONTROL PROJECT BOTH AP/Paradym EventShieldPro/DS-F8881 Documents/.claude/worktrees/pensive-cray/super6-website"
npm install
npm run dev          # http://localhost:3000 (Turbopack — hard-reload Cmd-Shift-R after CSS edits)
npm run build        # must pass before declaring work done
npm run lint         # 0 errors / 0 warnings on touched files
```

### Branch

Active working branch: `claude/pensive-cray` — pushed to `origin/claude/pensive-cray`. Default base is `main`. Never amend commits, never bypass git hooks, never push to `main`.

## Getting Started

First, run the development server:

```bash
npm run dev
# or
yarn dev
# or
pnpm dev
# or
bun dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

You can start editing the page by modifying `app/page.tsx`. The page auto-updates as you edit the file.

This project uses [`next/font`](https://nextjs.org/docs/app/building-your-application/optimizing/fonts) to automatically optimize and load [Geist](https://vercel.com/font), a new font family for Vercel.

## Learn More

To learn more about Next.js, take a look at the following resources:

- [Next.js Documentation](https://nextjs.org/docs) - learn about Next.js features and API.
- [Learn Next.js](https://nextjs.org/learn) - an interactive Next.js tutorial.

You can check out [the Next.js GitHub repository](https://github.com/vercel/next.js) - your feedback and contributions are welcome!

## Deploy on Vercel

The easiest way to deploy your Next.js app is to use the [Vercel Platform](https://vercel.com/new?utm_medium=default-template&filter=next.js&utm_source=create-next-app&utm_campaign=create-next-app-readme) from the creators of Next.js.

Check out our [Next.js deployment documentation](https://nextjs.org/docs/app/building-your-application/deploying) for more details.
