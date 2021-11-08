# README #

## Aqovia.CachingHttpClient

Nuget Info

[![NuGet Info](https://buildstats.info/nuget/Aqovia.CachingHttpClient?includePreReleases=true)](https://www.nuget.org/packages/Aqovia.CachingHttpClient/)

Build status

[![.NET Core](https://github.com/Aqovia/Aqovia.CachingHttpClient/workflows/Nuget%20Publish%20CI/badge.svg?branch=master)](https://github.com/Aqovia/Aqovia.CachingHttpClient/actions?query=branch%3Amaster)

[![Windows Build history](https://buildstats.info/github/chart/Aqovia/Aqovia.CachingHttpClient?branch=master&includeBuildsFromPullRequest=false)](https://github.com/Aqovia/Aqovia.CachingHttpClient/actions?query=branch%3Amaster)

## About


## Getting started

* Include the Nuget package in your test project
    - https://www.nuget.org/packages/Aqovia.CachingHttpClient/
* Either use the CachingHelper to create a CachingHttpClient or use ASP.NET Core DI to get HttpClient


## Project overview

The project contains the following directory structure

```
examples/
    Aqovia.CachingHttpClient.AspNetCoreApi
src/
    Aqovia.CachingHttpClient
test/
    Aqovia.CachingHttpClient.Tests
```

### src/Aqovia.CachingHttpClient

Contains the source library code providing a helper class to create ICacheStore and HttpClient with a default CacheStore.

### examples/Aqovia.CachingHttpClient.AspNetCoreApi

Example ASP.NET Core API to enable testing of the caching HttpClient

### test/Aqovia.CachingHttpClient.Tests

Contains a basic test running the AspNetCoreApi project and using a HttpClient created from the helpers to test that on a subsequent request the response comes from cache.

## Contributing

Assuming the repository is cloned and up-to-date (`master` branch)

1. Create a branch from `master` using `git checkout -b new_feature_branch`
2. Implement changes on new feature branch
3. Test and build locally - updating tests if required
4. Push to remote and fix any remote build/test issues
5. Create a pull request to the `master` branch
- include a well-formed title and description as these will be included in the release notes if/when the feature is merged to master
- include also in your description one of the following strings
    - 'bump: patch' - if this PR implements a new fix
    - 'bump: minor' - if this PR implements a new feature
    - 'bump: major' - if this PR implements a new feature with breaking changes

## Release Process

- The release process is automated by the CI process for every successful merge to master.
- The PR request title and description are used to create the Release note found via the `Releases` link on the repo landing page
- Inclusion of the keywords (bump: major|minor|patch) in the PR description is sufficient for the developer to control the upgrade to the final semantic version of the package
- Branch Preview packages are also available via the Aqovia Nuget OSS Feeds (publically available)
- Release packages are available on the [Nuget.org](https://www.nuget.org/packages/Aqovia.CachingHttpClient)
- Github release info is also available to view/compare and download source via the repo landing page

