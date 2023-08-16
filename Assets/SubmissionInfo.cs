
// This class contains metadata for your submission. It plugs into some of our
// grading tools to extract your game/team details. Ensure all Gradescope tests
// pass when submitting, as these do some basic checks of this file.
public static class SubmissionInfo
{
    // TASK: Fill out all team + team member details below by replacing the
    // content of the strings. Also ensure you read the specification carefully
    // for extra details related to use of this file.

    // URL to your group's project 2 repository on GitHub.
    public static readonly string RepoURL = "https://github.com/COMP30019/project-2-team-kisara";
    
    // Come up with a team name below (plain text, no more than 50 chars).
    public static readonly string TeamName = "Kisara";
    
    // List every team member below. Ensure student names/emails match official
    // UniMelb records exactly (e.g. avoid nicknames or aliases).
    public static readonly TeamMember[] Team = new[]
    {
        new TeamMember("Taelot Qian", "xingruiq@student.unimelb.edu.au"),
        new TeamMember("Yung-Ching Lin ", "yungchingl@student.unimelb.edu.au"),
        new TeamMember("Ryan Goh", "rrgoh@student.unimelb.edu.au"),
        // Remove the following line if you have a group of 3
        //new TeamMember("Zois Stavrakas", "zstavtakas@student.unimelb.edu.au"), 
    };

    // This may be a "working title" to begin with, but ensure it is final by
    // the video milestone deadline (plain text, no more than 50 chars).
    public static readonly string GameName = "No Time to Die";

    // Write a brief blurb of your game, no more than 200 words. Again, ensure
    // this is final by the video milestone deadline.
    public static readonly string GameBlurb = 
@"Playing as a wounded lone knight, looking for food and supplies in a endless desert, dies if you don't eat within 30 secends. 
Picking up powerful items, defeating enemies and get updated.
Food and supplies are spawning randomly in the map and disappears in a minute.
One suggestion, Don't die!
";
    
    // By the gameplay video milestone deadline this should be a direct link
    // to a YouTube video upload containing your video. Ensure "Made for kids"
    // is turned off in the video settings. 
    public static readonly string GameplayVideo = "https://youtu.be/JTKhzQEPLWQ";
    
    // No more info to fill out!
    // Please don't modify anything below here.
    public readonly struct TeamMember
    {
        public TeamMember(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }
        public string Email { get; }
    }
}
