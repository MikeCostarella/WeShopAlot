# The build gate

What happens to a pull request on this repository, what each check means, and what to do when one is red. The class fork is [MyWebSiteParticipants/WeShopAlot](https://github.com/MyWebSiteParticipants/WeShopAlot); the same files work on any fork once its owner turns on the settings in the last section.

## What runs on every pull request

| Check | Comes from | What it does | Typical time |
|---|---|---|---|
| **api** | `.github/workflows/ci.yml` | Builds the Web API and runs `WeShopAlot.Testing.WebApis` on a clean Ubuntu machine. The tests run the whole API in memory, so no database, Redis or secrets are needed. | under a minute |
| **client** | `.github/workflows/ci.yml` | `npm ci`, `ng build`, `ng test` for the Angular client in `Solution/Client`. | under a minute |
| **scan** | `.github/workflows/secret-scan.yml` | Fails if the pull request adds or changes any `.env` file (except `.env.example`), then runs [TruffleHog](https://github.com/trufflesecurity/trufflehog) over the pull request's commits looking for API keys, tokens and connection strings. | seconds |
| **CodeRabbit** | `.coderabbit.yaml` + the CodeRabbit GitHub App | An AI review: a summary of the change and line comments. Its rules for this repo: no secrets in `appsettings*.json` or Angular `environments/`, no `.env` files, no leftover `[insert …]` placeholders in docs, and plain-language explanations because this is a teaching codebase. | a few minutes |

The WPF client is not built in CI. It needs Windows, and the runners are Linux. Build it locally before you open a pull request that touches `WeShopAlot.UI.ClientWPF`.

The first three are **required**: the ruleset on `master` (see below) disables the Merge button until `api`, `client` and `scan` are green. CodeRabbit's comments are advice; they block nothing by themselves, but a reviewer will ask why an unanswered one was ignored.

## When a check is red

Open the pull request's **Checks** tab, click the red job, and read the log from the top. The first error line is the one that matters; everything after it is usually fallout.

**api is red**

- A compile error: the log shows the file and line. It compiles on your machine? Then something is not committed. `git status` and look for a file you forgot to add.
- A test failed: the log names the test. Run `dotnet test` in `Solution` yourself; the API tests need nothing installed beyond the .NET SDK.
- A case-sensitivity problem: `WeShopAlot..WebAPI` and `WeShopAlot.Data` are spelled exactly that way on Linux. Windows forgives `weshopalot.data`; the runner does not.

**client is red**

- `npm ci` failed: your `package-lock.json` does not match `package.json`. Run `npm install` in `Solution/Client`, commit the updated lock file.
- `ng build` failed: a TypeScript error the dev server tolerated. The log shows file and line.
- `ng test` failed: run `npx ng test --watch=false` in `Solution/Client` to see it locally.

**scan is red**

Two different things can cause it, and they have different fixes.

1. *"This pull request adds or changes environment files"* — you committed a `.env` file. The fix is not just deleting it: if the file held real values, **the values are in git history now** and must be rotated. See the next section.
2. *TruffleHog found a secret* — the log names the file, the line and the kind of key. Same rule: rotate first, then clean the branch.

**CodeRabbit left a comment you disagree with**

Reply in the thread and say why, with evidence (a test, a doc link, the line that already handles it). Mention `@coderabbitai` in the reply if you want it to reconsider. Mark it resolved when you are done. Do not resolve a comment silently; the reviewer reads the threads.

## If you pushed a secret

Deleting the file in the next commit does not remove the secret. Every commit stays in the branch's history, in the pull request's Commits tab, and in every clone made in between. The order is:

1. **Rotate.** Make a new key at the provider (Stripe, Azure, wherever) and revoke the old one. Do this first; the old key is live until it is revoked.
2. **Put the new value where it belongs.** For the API, `dotnet user-secrets` (see the README's *Secrets* section). Never in a file that is committed.
3. **Clean the branch.** If the secret is in one commit, `git rm --cached <file>` and commit. If it is spread across several, start a fresh branch from `master` and bring over only the code.
4. **Say so on the pull request.** A red scan fixed by rotation is a normal day. A red scan fixed by deleting the file and force-pushing is an incident waiting to be found.

GitHub's own **push protection** is also on for this repository. If your `git push` is refused with `GITHUB PUSH PROTECTION`, GitHub recognized a real key in your commit and the commit never left your machine. Fix the file, amend the commit, push again. No rotation is needed in that case, because nothing was exposed.

## Where secrets go instead

| Value | Where it lives | Why |
|---|---|---|
| `Token:Key`, `StripeSettings:SecretKey`, `StripeSettings:WhSecret`, connection strings, `AutoMapper:LicenseKey` | `dotnet user-secrets` in the API project (and the seeder) | Outside the repository, per developer. See the README. |
| Local overrides for scripts or tools | `.env.local` or `.env` — **ignored by git** | `.gitignore` keeps them out; commit a `.env.example` with placeholders instead. |
| The Stripe **publishable** key (`pk_test_…`) | `appsettings.json` and `Client/src/environments/` | Public by design. It is fine in the repo. |
| Anything in `Client/src/environments/` | Shipped to the browser | Everyone can read it, so only public values belong there. |

## The ruleset on `master`

The fork's *Protect master* ruleset (Settings → Rulesets) says:

- No direct pushes: changes reach `master` through a pull request.
- `api`, `client` and `scan` must pass before a merge.
- No force pushes; the branch cannot be deleted.
- **Bypass list:** repository and organization admins. An admin's direct push succeeds and is logged as *Bypassed rule violations*, so an admin cannot test the ruleset by pushing. A non-admin's push being refused is the proof it works.

If the Merge button says the branch is out of date, click **Update branch** on the pull request. That merges the latest `master` into your branch and reruns the checks.

## Running the checks locally

Both CI jobs can be reproduced on your machine before you push.

PowerShell:

```powershell
cd C:\projects\WeShopAlot\Solution
dotnet build "WeShopAlot..WebAPI\WeShopAlot.WebAPI.csproj" --configuration Release
dotnet test WeShopAlot.Testing.WebApis\WeShopAlot.Testing.WebApis.csproj --configuration Release
cd Client
npm ci
npx ng build
npx ng test --watch=false
```

For the secret scan, TruffleHog has a Windows build; the quickest check is simpler: `git status` before every commit, and never `git add -f` a file that `.gitignore` is refusing.

## Setting this up on another fork

The workflow files, `.coderabbit.yaml` and `.gitignore` travel with the code. The rest are settings an admin turns on once per repository:

1. **Actions** tab: if GitHub says workflows are off for this fork, click *I understand my workflows, go ahead and enable them*.
2. **Settings → Advanced Security:** *Enable* next to **Secret Protection**, then *Enable* next to **Push protection**. Free on public repositories.
3. **CodeRabbit:** the app is installed on the MyWebSiteParticipants organization. Org Settings → GitHub Apps → coderabbitai → *Repository access* → add the repository → Save. Then *Sync Repositories* in the CodeRabbit dashboard.
4. **Settings → Rulesets → New branch ruleset:** target the default branch; require a pull request; require status checks `api`, `client`, `scan`; keep *Block force pushes* and *Restrict deletions*; add the admin roles to the bypass list; set it *Active*. Create it after the checks have run at least once, so the *Add checks* list offers them.
5. **Settings → General → Pull Requests:** tick *Always suggest updating pull request branches*, so the *Update branch* button appears.

A pull request that was already open when the workflows arrived shows no checks until something happens to it. Click *Update branch* on it, or close and reopen it.
