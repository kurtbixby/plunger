namespace Plunger.WebApi.EndpointContracts;

public record ListUpdateRequest()
{
    public record ListUpdate
    {
        public ListUpdateAction Action { get; init; }
        
        // Can be a number
        public string Payload { get; init; }
    }

    public enum ListUpdateAction
    {
        ChangeName = 1,
        ChangeOrdered = 2,
        AddGame = 10,
        RemoveGame = 11,
        MoveGame = 12
    }
    
    public List<ListUpdate> Updates { get; init; }
    public Guid VersionId { get; init; }

    public ValidationResult Validate()
    {
        var result = new ValidationResult() { ValidationErrors = new Dictionary<string, string>() };
        if (Updates.Count < 1)
        {
            result.IsValid = false;
            result.ValidationErrors["updateCount"] = "No updates in request";
        }

        foreach (var update in Updates)
        {
            if (String.IsNullOrWhiteSpace(update.Payload))
            {
                result.IsValid = false;
                result.ValidationErrors["updatePayload"] = "An update payload is blank";
                break;
            }
        }

        return result;
    }
}


/*
Change name
Change ordered/unordered

Add game
Remove game
Reorder game
*/