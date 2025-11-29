# Contributing to LiteHttp

Thanks for wanting to improve LiteHttp. Keep it simple, keep it clean. This template is minimal so you can tweak it for your workflow.

## How to Contribute

### Reporting Bugs or Requesting Features

Before implementing anything, open an issue.  
One issue covers only **one** exact problem. If there is related problem, it's better to open a new issue.  
Describe the problem, expected vs. actual behavior, environment, and minimal reproducible example.  
For features, include motivation and a proposed API design.  
If an issue already exists upvote it (👍 reaction)

### Opening a Pull Request

1. Create a fork.
2. Create a new branch from `main` using a clear name like `feature/<name>` or `fix/<name>`.
3. Make small, focused commits.
4. Run all tests and formatting tools locally before pushing.
5. Clearly describe what the PR changes and why.
6. Update documentation or changelog if required.

## Tests and CI

Every meaningful change must include tests.  
Bug fixes require regression tests.  
CI must pass — no exceptions.

## Code Style and Formatting

The project follows the repository’s `.editorconfig` / linting setup.   
Fix all warnings associated with style conventions of this project.   
Use clear naming, avoid hidden side effects, and document only when logic isn’t obvious.  

## Commit Messages and Branches

Use semantic, descriptive commits (e.g., Fix(startup): exception occurring during startup #12345; Add 402 response code support).  
One PR = one logical purpose. Don’t mix unrelated changes.

## Security and Vulnerabilities

Do **not** disclose security issues publicly.  
Report them through a private channel (email to repository owner etc.).  
Security-sensitive changes require additional review and tests covering exploit scenarios.

## Documentation

Documentation should stay minimal and consistent.  
Use XML documentation in your code for clarity.  
Discuss updating the README or other documentation before modifying it. This rule **does not apply to typo fixes**.

## Release Process

The project uses semantic versioning.  
For each release: update the changelog, tag the release, and ensure CI passes.  
Automated publishing may run depending on repository configuration.

## Support and Communication

Use issues for questions, bug reports, and proposals.  
