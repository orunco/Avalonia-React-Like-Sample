# Avalonia-React-Like-Sample

A sample project demonstrating React-like component-based UI development in Avalonia framework.

## What？

From a human-understandable perspective, UI development based on the React model should be the optimal solution:

- **Simple concept**: Everything is a component (Component)
- **Simple implementation**
- **JSX**: Use the powerful capabilities of JavaScript to express HTML structure

The MVVM pattern in Avalonia or WPF might actually be too complex. Therefore, I hope to use this sample to explore how to implement Avalonia using the React pattern.

## Development Verification

| No. | Content                                                  | Implementation                                                                 |
|-----|-----------------------------------------------------------|--------------------------------------------------------------------------------|
| 1   | Use React-style Component implementation, preferably FC (Function Component) in a single file | Based on `UserControl + ReactiveUI`, e.g., `Footer.cs`, implemented mainly in constructor; avoid maintaining state inside the class |
| 2   | axaml                                                     | Implemented via code, as axaml is difficult for AI to generate effectively due to lack of training data |
| 3   | CSS Styled                                                | Implemented via code; CSS is also unlikely to be used since most code is AI-generated, same reason as above — better to implement with code |
| 4   | Hook                                                      | Refer to `Hook` for a simple implementation. However, fully replicating it is difficult and increases complexity, making it hard to understand — simplicity may be better |
| 5   | Parent component calling child component                  | Parameter passing via constructor                                              |
| 6   | Child component calling parent component                  | Implemented via callback functions                                             |
| 7   | Sibling component communication                           | Implemented via a common parent component                                      |
| 8   | Cross-component communication                             | Not yet implemented                                                            |
|     | Theme                                                     | Validated using Semi                                                           |
|     |                                                           |                                                                                |

## Testing Verification

| No. | Content         | Status |
|-----|------------------|--------|
| 1   | Unit Test (UT)   | OK     |
| 2   | Headless Test    | OK     |
 