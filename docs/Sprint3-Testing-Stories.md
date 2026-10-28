# Sprint 3 testing stories (Nov 2 – Nov 13)

Backlog items for the WeShopAlot board, written as GitHub issues: copy a title into
the issue title and the block under it into the body. Each is one pull request into
`dev` from a branch named as shown. Module 6 (unit testing) is the lecture behind
them; `WeShopAlot.Testing.Unit` is the project they grow.

Done already, in the PR that opened this project: `ProductSpecParams`,
`ProductsWithTypesAndBrandsSpecification`, `SpecificationEvaluator`, `Order`
defaults and `MappingProfiles` tests, the `Search = null` fix, and CI running
the unit project first.

---

## 1. PageIndex below 1 produces a negative Skip

**Branch:** `test/spec-params-page-index` · **Size:** S · **Label:** bug, testing

`ProductsWithTypesAndBrandsSpecification` computes `Skip = PageSize * (PageIndex - 1)`.
`GET /api/product?pageIndex=0` therefore asks for `Skip(-6)`. LINQ-to-Objects treats
that as 0; SQL Server's `OFFSET -6 ROWS` does not.

**Acceptance**
- [ ] A failing unit test first: `ProductSpecParams { PageIndex = 0 }` (and `-1`) must behave as page 1.
- [ ] Decide where the clamp lives (the `PageIndex` setter, like the `PageSize` cap) and implement it.
- [ ] Add the boundary trio for the cap while you are there: 49, 50, 51.
- [ ] `dotnet test WeShopAlot.Testing.Unit` green; `ApiTests` still green.

## 2. Secondary sort for equal prices

**Branch:** `test/spec-sort-ties` · **Size:** S · **Label:** testing, design

`priceAsc` and `priceDesc` replace the name ordering entirely, so two products at
the same price come back in whatever order the database likes. The constructor also
calls `AddOrderBy(x => x.Name)` and then overwrites it, which reads like a bug.

**Acceptance**
- [ ] A test in `SpecificationEvaluatorTests` with two products at the same price that pins the order you want (name as the tie-breaker is the obvious choice).
- [ ] `BaseSpecification` grows a `ThenBy` (or the evaluator applies name after price) so the test passes.
- [ ] Remove the dead `AddOrderBy(x => x.Name)` call if it no longer does anything, and say so in the PR.

## 3. MappingProfiles passes AssertConfigurationIsValid

**Branch:** `test/mapping-config-valid` · **Size:** M · **Label:** testing

`MappingProfilesTests` builds the configuration but never calls
`AssertConfigurationIsValid()`. Add that call as its own test and make it pass.
Expect it to fail first: destination members such as `Address.Id`, `Address.IsDeleted`
and `CustomerBasket.ClientSecret` have no source. Each one is either `Ignore()` with
a reason, or a real missing mapping.

**Acceptance**
- [ ] `Configuration_IsValid` test exists and is green.
- [ ] Every `Ignore()` added has a one-line comment saying why the member is not mapped.
- [ ] No behavior change to the API (`ApiTests` green).

## 4. TokenService unit tests

**Branch:** `test/token-service` · **Size:** S · **Label:** testing

`TokenService.CreateToken` needs only an `IConfiguration` with `Token:Key` and
`Token:Issuer`. Build one in memory, create a token, parse it back with
`JwtSecurityTokenHandler`, and assert on what the API's authentication checks.

**Acceptance**
- [ ] Tests in `WeShopAlot.Testing.Unit/Services/TokenServiceTests.cs`: issuer matches configuration; name and email claims present; expiry about one day out; signed with HS512.
- [ ] Remove the empty `try { } catch { throw; }` around `WriteToken` (it does nothing) in the same PR, since the tests now cover the method.

## 5. Server-side price rule as a pure function

**Branch:** `test/order-subtotal-pure` · **Size:** M · **Label:** testing, refactor

`Checkout_BasketToOrder_UsesServerPrices` proves over HTTP that an order uses catalog
prices, not the client's. The arithmetic lives inside `OrderService.CreateOrderAsync`
next to repository calls, so it can only be tested by booting the API.

**Acceptance**
- [ ] Extract the item-pricing and subtotal step into a pure static method (for example `OrderPricing.Price(basketItems, products)`), with no repository or DbContext parameter.
- [ ] Unit tests: client price ignored; quantity multiplies; subtotal sums; an item whose product is missing is a clear failure, not a silent zero.
- [ ] `OrderService` calls the new method; `ApiTests` still green (it is the safety net for this refactor).

## 6. Angular: calculateTotals as a pure function

**Branch:** `test/basket-totals-pure` · **Size:** S · **Label:** testing, client

`BasketService.calculateTotals()` is private and reached only through a flushed HTTP
request in `basket.service.spec.ts`. The same move the lecture made with Bullpen's
risk guard applies: pull the arithmetic out.

**Acceptance**
- [ ] `src/app/shared/models/basket.ts` (or a new `basket-totals.ts`) exports `calculateTotals(basket: Basket): BasketTotals`.
- [ ] `basket-totals.spec.ts` covers: empty basket; one item × quantity; shipping added; two items summed. No `TestBed`, no `HttpTestingController`.
- [ ] `BasketService` calls it; existing specs unchanged and green (`npx ng test --watch=false`).

## 7. Retire the empty test projects

**Branch:** `chore/empty-test-projects` · **Size:** S · **Label:** housekeeping

`WeShopAlot.Testing.Services`, `WeShopAlot.Testing.Repositories` (namespace
`OurGovRepositoryTests`) and `WeShopAlot.Testing.XSLT` each hold one `TestMethod1`
that asserts nothing. `WeShopAlot.Testing.Shared` carries `OurGov.*` namespaces and a
`Dispose()` that throws `NotImplementedException`.

**Acceptance**
- [ ] Either delete the three projects and `Testing.Shared` from the solution, or give each a first real test — decide in the PR description, not silently.
- [ ] No `OurGov` namespace remains in the repo (`git grep OurGov` is empty).
- [ ] Solution builds; CI green.

## 8. Test results and coverage visible on every PR

**Branch:** `ci/test-reports` · **Size:** M · **Label:** ci

CI runs the suites but a reviewer has to open the log to see counts. Publish them.

**Acceptance**
- [ ] `dotnet test` runs with `--logger trx --collect:"XPlat Code Coverage"` for both .NET test projects and the results are uploaded as workflow artifacts.
- [ ] A test-summary step (for example `dorny/test-reporter` or `EnricoMi/publish-unit-test-result-action`) posts pass/fail counts on the PR.
- [ ] Angular: `ng test --watch=false --code-coverage` and the `coverage/` folder uploaded the same way.
- [ ] `docs/BuildGate.md` updated with where to find the reports.
