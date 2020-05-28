
# How to contribute
I'm really glad you're reading this.

## Bugs?
Plz report it in an issue labelled as [![](https://i.imgur.com/JflcH01.png)](../labels/bug) - in the corresponding repo.

## Testing
I have a handful of unit and integration tests in the sample codes.
Please write at least one or two for new code you create.
Please follow our coding conventions (below). Please use the following tools for C# code automated testing:
 + [MSTest](https://github.com/microsoft/testfx)
 + [NSubstitute](https://nsubstitute.github.io)
 + [Fluent Assertions](https://fluentassertions.com)

## Submitting changes
Please send a GitHub Pull Request with a clear list of what you've done (read more about [pull requests](http://help.github.com/pull-requests)).
When you send a pull request, we will love you forever if you include automated tests. We can always use more test coverage.
Please follow our coding conventions (below) and make sure all of your commits are atomic (one feature per commit).

Always write a clear log message for your commits. I love the following very short commit message format:

` + apple` - feature / module / class / whatever `apple` added <br />
` - apple` - feature / module / class / whatever `apple` removed <br />
`apple ++` - feature / module / class / whatever `apple` improved / extended <br />
`apple --` - feature / module / class / whatever `apple` reduced / simplified <br />
`fixed #123` - issue #123 fixed - see also &rarr; [Closing Issues via Commit Messages](https://github.blog/2013-01-22-closing-issues-via-commit-messages)

## Coding conventions

Start reading our code and you'll get the hang of it. We optimize for readability:

  * This is an open source sample code collection. Consider the people who will read your code, and make it look nice for them. It's sort of like driving a car: Perhaps you love doing donuts when you're alone, but with passengers the goal is to make the ride as smooth as possible.
  * Please respect code CA rules where provided - like:
     * ReSharper settings
     * StyleCop rules
     * `.editorconfig` - incl. FxCop - rules

### Unit and integration test conventions
#### Test project name
`<tested class name>.Tests` - e.g.:

`DomainModel.Domain.Tests`

#### Test class name
`<tested class name>UnitTests` / `<tested class name>IntegrationTests` - e.g.:

```c#
[TestClass]
public class BillUnitTests
```
```c#
[TestClass]
public class CheckoutServiceIntegrationTests
```

#### Test method name
`<tested method name>_Should<do something>` / `<tested method name>_Should<do something>_When<some condition>` - e.g.:

```c#
[TestMethod]
public void ApplyDiscounts_ShouldDecreaseTotalPrice()
```
```c#
[TestMethod]
public void ShowBill_ShouldGiveEmptyBill_WhenNothingScannedYet()
```

#### AAA pattern
Please apply the [AAA pattern](https://www.thephilocoder.com/unit-testing-aaa-pattern) in all automated test cases - e.g.:

```c#
[TestMethod]
public void ShowBill_ShouldGiveEmptyBill_WhenNothingScannedYet()
{
    // Arrange
    var outChecker = CreateDefaultOutChecker();
    outChecker.Start();

    // Act
    var bill = outChecker.ShowBill();

    // Assert
    bill.Should().Be(Bill.EmptyBill);
}
```

Thanks,
[Gustin AG](https://gustinsblog.wordpress.com)
