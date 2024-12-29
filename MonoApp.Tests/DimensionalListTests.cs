namespace MonoApp.Geometry;

/*
    Feature Specification

    A DimensionalList is a list whose elements are accessible and modifiable through 
    indexing chunks.

    The size of a DL is fixed.
        var list = new DimensionalList([2, 2]);
        Assert.Equal(4, list.Count);

    The DL is filled with default value for its type if no values are supplied.
        var list = new DimensionalList([3, 2]);
        Assert.Equal([0, 0, 0, 0, 0, 0], list.Elements);

    The DL is indexible through an IEnumerable<int>
        var list = new DimensionalList[3, 2], [0, 1, 2, 3, 4, 5]);
        Assert.Equal([0, 1], list[[0]]);
        Assert.Equal([2, 3], list[[1]]);
        Assert.Equal([4, 5], list[[2]]);
        Assert.Equal([0], list[[0, 0]]);
        Assert.Equal([1], list[[0, 1]]);
        Assert.Equal([2], list[[1, 0]]);
        Assert.Equal([3], list[[1, 1]]);
        Assert.Equal([4], list[[2, 0]]);
        Assert.Equal([5], list[[2, 1]]);

    A subrange of the values of a DL can be set by indexing
        var list = new DimensionalList([3, 2]);
        Assert.Equal([0, 0, 0, 0, 0, 0], list.Elements);

        list[[0]] = [5, 6];
        Assert.Equal([5, 6, 0, 0, 0, 0], list.Elements);

        list[[2, 1]] = [3];
        Assert.Equal([5, 6, 0, 0, 0, 3], list.Elements);

*/
public class DimensionalListTests
{
    [Fact]
    public void Should_Be_Constructable_With_Dimensions()
    {
        var list = new DimensionalList<int>([3]);
        var dimensions = list.Dimensions;
        Assert.Equal([3], dimensions);
    }

    public static TheoryData<DimensionalList<int>, IEnumerable<int>> Data0 => new()
    {
        {
            new DimensionalList<int>([3], Enumerable.Range(0, 3).Select(i => i)),
            new List<int>() { 0, 1, 2 }
        },
        {
            // Any missing values are filled with default of T
            new DimensionalList<int>([3]),
            new List<int>() { 0, 0, 0}
        },
        {
            // Any extra values are discarded
            new DimensionalList<int>([2, 2], Enumerable.Range(0, 6).Select(i => i)),
            new List<int>() { 0, 1, 2, 3 }
        }
    };

    [Theory]
    [MemberData(nameof(Data0))]
    public void Should_Be_Constructable_With_Any_Number_Of_Elements
    (DimensionalList<int> list, IEnumerable<int> elements)
    {
        Assert.Equal(elements, list.Elements);
    }

    public static TheoryData<int[]> Data1 => new()
    {
        { [3, 2] },
        { [5, 4] }
    };

    public static TheoryData<DimensionalList<int>, int> Data2 => new()
    {
        { new([3, 2]), 6},
        { new([5, 4]), 20 }
    };

    [Fact]
    public void Should_Be_Implicitly_Convertible_To_List_Of_T()
    {
        var list = new DimensionalList<int>([3, 2]);
        Assert.Equal(new List<int>() { 0, 0, 0, 0, 0, 0 }, list);
    }

    [Fact]
    public void StartIndices()
    {
        var list = new DimensionalList<int>([4, 3, 2]);
        Assert.Equal(0, list.StartIndex([0]));
        Assert.Equal(6, list.StartIndex([1]));
        Assert.Equal(12, list.StartIndex([2]));
        Assert.Equal(18, list.StartIndex([3]));

        Assert.Equal(0, list.StartIndex([0, 0]));
        Assert.Equal(2, list.StartIndex([0, 1]));
        Assert.Equal(4, list.StartIndex([0, 2]));
        Assert.Equal(6, list.StartIndex([1, 0]));
        Assert.Equal(8, list.StartIndex([1, 1]));
        Assert.Equal(10, list.StartIndex([1, 2]));
        Assert.Equal(12, list.StartIndex([2, 0]));
        Assert.Equal(14, list.StartIndex([2, 1]));
        Assert.Equal(16, list.StartIndex([2, 2]));
        Assert.Equal(18, list.StartIndex([3, 0]));
        Assert.Equal(20, list.StartIndex([3, 1]));
        Assert.Equal(22, list.StartIndex([3, 2]));

        Assert.Equal(0, list.StartIndex([0, 0, 0]));
        Assert.Equal(1, list.StartIndex([0, 0, 1]));
        Assert.Equal(2, list.StartIndex([0, 1, 0]));
        Assert.Equal(3, list.StartIndex([0, 1, 1]));
        Assert.Equal(4, list.StartIndex([0, 2, 0]));
        Assert.Equal(5, list.StartIndex([0, 2, 1]));

        Assert.Equal(6, list.StartIndex([1, 0, 0]));
        Assert.Equal(7, list.StartIndex([1, 0, 1]));
        Assert.Equal(8, list.StartIndex([1, 1, 0]));
        Assert.Equal(9, list.StartIndex([1, 1, 1]));
        Assert.Equal(10, list.StartIndex([1, 2, 0]));
        Assert.Equal(11, list.StartIndex([1, 2, 1]));

        Assert.Equal(12, list.StartIndex([2, 0, 0]));
        Assert.Equal(13, list.StartIndex([2, 0, 1]));
        Assert.Equal(14, list.StartIndex([2, 1, 0]));
        Assert.Equal(15, list.StartIndex([2, 1, 1]));
        Assert.Equal(16, list.StartIndex([2, 2, 0]));
        Assert.Equal(17, list.StartIndex([2, 2, 1]));

        Assert.Equal(18, list.StartIndex([3, 0, 0]));
        Assert.Equal(19, list.StartIndex([3, 0, 1]));
        Assert.Equal(20, list.StartIndex([3, 1, 0]));
        Assert.Equal(21, list.StartIndex([3, 1, 1]));
        Assert.Equal(22, list.StartIndex([3, 2, 0]));
        Assert.Equal(23, list.StartIndex([3, 2, 1]));
    }

    [Fact]
    public void ChunkSizes()
    {
        var list = new DimensionalList<int>([4, 3, 2]);
        Assert.Equal(6, list.ChunkSize([0]));
        Assert.Equal(6, list.ChunkSize([1]));
        Assert.Equal(6, list.ChunkSize([2]));
        Assert.Equal(6, list.ChunkSize([3]));

        Assert.Equal(2, list.ChunkSize([0, 0]));
        Assert.Equal(2, list.ChunkSize([0, 1]));
        Assert.Equal(2, list.ChunkSize([0, 2]));
        Assert.Equal(2, list.ChunkSize([1, 0]));
        Assert.Equal(2, list.ChunkSize([1, 1]));
        Assert.Equal(2, list.ChunkSize([1, 2]));
        Assert.Equal(2, list.ChunkSize([2, 0]));
        Assert.Equal(2, list.ChunkSize([2, 1]));
        Assert.Equal(2, list.ChunkSize([2, 2]));
        Assert.Equal(2, list.ChunkSize([3, 0]));
        Assert.Equal(2, list.ChunkSize([3, 1]));
        Assert.Equal(2, list.ChunkSize([3, 2]));

        Assert.Equal(1, list.ChunkSize([0, 0, 0]));
        Assert.Equal(1, list.ChunkSize([0, 0, 1]));
        Assert.Equal(1, list.ChunkSize([0, 1, 0]));
        Assert.Equal(1, list.ChunkSize([0, 1, 1]));
        Assert.Equal(1, list.ChunkSize([0, 2, 0]));
        Assert.Equal(1, list.ChunkSize([0, 2, 1]));
        Assert.Equal(1, list.ChunkSize([1, 0, 0]));
        Assert.Equal(1, list.ChunkSize([1, 0, 1]));
        Assert.Equal(1, list.ChunkSize([1, 1, 0]));
        Assert.Equal(1, list.ChunkSize([1, 1, 1]));
        Assert.Equal(1, list.ChunkSize([1, 2, 0]));
        Assert.Equal(1, list.ChunkSize([1, 2, 1]));
        Assert.Equal(1, list.ChunkSize([2, 0, 0]));
        Assert.Equal(1, list.ChunkSize([2, 0, 1]));
        Assert.Equal(1, list.ChunkSize([2, 1, 0]));
        Assert.Equal(1, list.ChunkSize([2, 1, 1]));
        Assert.Equal(1, list.ChunkSize([2, 2, 0]));
        Assert.Equal(1, list.ChunkSize([2, 2, 1]));
        Assert.Equal(1, list.ChunkSize([3, 0, 0]));
        Assert.Equal(1, list.ChunkSize([3, 0, 1]));
        Assert.Equal(1, list.ChunkSize([3, 1, 0]));
        Assert.Equal(1, list.ChunkSize([3, 1, 1]));
        Assert.Equal(1, list.ChunkSize([3, 2, 0]));
        Assert.Equal(1, list.ChunkSize([3, 2, 1]));
    }


    [Fact]
    public void Subranges_Should_Be_Accessible_By_Indexing()
    {
        var list = new DimensionalList<int>([3, 2], [0, 1, 2, 3, 4, 5]);
        Assert.Equal([0, 1], list[[0]]);
        Assert.Equal([2, 3], list[[1]]);
        Assert.Equal([4, 5], list[[2]]);
        Assert.Equal([0], list[[0, 0]]);
        Assert.Equal([1], list[[0, 1]]);
        Assert.Equal([2], list[[1, 0]]);
        Assert.Equal([3], list[[1, 1]]);
        Assert.Equal([4], list[[2, 0]]);
        Assert.Equal([5], list[[2, 1]]);
    }

    [Fact]
    public void Subranges_Should_Be_Settable_By_Indexing()
    {
        var list = new DimensionalList<int>([3, 2]);
        Assert.Equal([0, 0, 0, 0, 0, 0], list.Elements);

        list[[0]] = [5, 6];
        Assert.Equal([5, 6, 0, 0, 0, 0], list.Elements);

        list[[2, 1]] = [3];
        Assert.Equal([5, 6, 0, 0, 0, 3], list.Elements);
    }
}