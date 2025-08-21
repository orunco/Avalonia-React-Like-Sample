# Avalonia-React-Like-Sample

A sample project demonstrating React-like component-based UI development in Avalonia framework.

## What？

From a human-understandable perspective, UI development based on the React model should be the optimal solution:

- **Simple concept**: Everything is a component
- **Simple implementation**
- **JSX**: Use the powerful capabilities of JavaScript to express HTML structure

The MVVM pattern in Avalonia or WPF might **actually be too complex**. Therefore, I hope to use this sample to explore how to implement Avalonia using the React pattern.

## Key Features

### 🧩 React-Like Component Architecture
- **Function Components**: Pure C# classes implementing component logic in single files
- **Component Props**: Data and callbacks passed via constructors and events
- **Component Hierarchy**: Parent-to-child and child-to-parent communication patterns
- **No XAML Templates**: Pure code-based UI construction for better AI generation support

### ⚡ Reactive Programming
- **ReactiveUI Integration**: Built-in reactive state management
- **Observable Properties**: Automatic UI updates with `[Reactive]` attributes
- **Computed Properties**: Derived values with `ObservableAsPropertyHelper`
- **Collection Observability**: Reactive collections with `ObservableCollectionExtended`

### 🎨 Modern UI Development
- **Component-Level Styling**: Each component has its own AXAML file for styling (similar to styled-components)
- **Component Encapsulation**: Self-contained components with internal logic

### 🧪 Comprehensive Testing
- **Unit Testing**: Component-level testing with direct control access
- **Headless UI Testing**: End-to-end testing with `AvaloniaUITest`
- **Visual Test Robots**: Dedicated robot classes for UI interaction testing
- **Test Automation**: Reliable UI automation with custom extensions

### 🔧 Development Benefits
- **AI-Friendly**: Code-only approach works better with AI code generation
- **Easy Debugging**: Component logic centralized in single files
- **Type Safety**: Full C# type safety for props and callbacks
- **Memory Management**: Automatic resource disposal with `XUserControl`

### 🗑️ Complete Resource Disposal
- **Automatic Cleanup**: `XUserControl` base class provides automatic disposal of subscriptions and resources
- **CompositeDisposable**: Built-in `CompositeDisposable` management for reactive subscriptions
- **Event Unregistration**: Automatic event handler registration and cleanup
- **Logical Tree Integration**: Disposal triggered when components detach from logical tree
- **Manual Disposal Support**: Full `IDisposable` implementation for explicit resource management

