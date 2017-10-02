using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterFeedSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            // fetch all the users with thier followers
            Dictionary<string, List<string>> user_follower = GetUsersAndTheirFollowers();

            //get all users, (users and followers are all users)
            List<string> userList = GetUsers(user_follower);

            // fetch users and their tweets
            Dictionary<string, List<string>> user_tweets = GetUserTweets();

            //Display output as required
            DiplayOutput(user_tweets,user_follower, userList);
        }
        public static Dictionary<string, List<string>> GetUserTweets()
        {
            Dictionary<string, List<string>> user_tweets = new Dictionary<string, List<string>>();
            var delimeter = '>';
            string fileName = "tweet.txt";
            string[] lines = ReadFile(fileName);

            foreach (var line in lines)
            {
                string[] tweetList = line.Split(delimeter);

                List<string> userTweetList = new List<string>();
                if (!user_tweets.ContainsKey(tweetList[0]))
                {
                    userTweetList.Add(tweetList[1]);
                    user_tweets.Add(tweetList[0], userTweetList);
                }
                if (user_tweets.ContainsKey(tweetList[0]) && !user_tweets.ContainsValue(userTweetList))
                {
                    user_tweets[tweetList[0]].Add(tweetList[1]);
                }
            }
            return user_tweets;
        }
        public static List<string> GetUsers(Dictionary<string, List<string>> userDictionary)
        {
            var user = "";
            var follower = "";
            List<string> userList = new List<string>();

            foreach (var userKey in userDictionary)
            {
                user = userKey.Key;
                if (!userList.Contains(user.Trim()) && !String.IsNullOrWhiteSpace(user))
                {
                    userList.Add(user.Trim());
                }
                foreach (var userValue in userKey.Value)
                {
                    follower = userValue.Trim();
                    if(!userList.Contains(follower.Trim())&& !String.IsNullOrWhiteSpace(follower))
                    {
                        userList.Add(follower.Trim());
                    }
                }
            }
            //order the list of users by ascending order
            return userList.OrderBy(x => x).ToList();
        }
        public static Dictionary<string, List<string>> GetUsersAndTheirFollowers()
        {
            var user = "";
            var follower = "";
            var delimeterString = "follows";
            var delimeter = ',';

            List<string> userList = new List<string>();
            Dictionary<string, List<string>> user_follower = new Dictionary<string, List<string>>();
            string fileName = "user.txt";
            string[] lines = ReadFile(fileName);

            foreach (var line in lines)
            {
                //split the lines with the "follows" string
                string[] userFollowerList = line.Split(new[] { delimeterString }, StringSplitOptions.None);

                follower = userFollowerList[0].Trim();
                user = userFollowerList[1].Trim();

                string[] users = user.Split(delimeter);

                if (users.Length > 1)
                {
                    foreach (var item in users)
                    {
                        List<string> followers = new List<string>();
                        followers.Add(follower);
                        if (!user_follower.ContainsKey(item.Trim()))
                        {
                            user_follower.Add(item.Trim(), followers);
                        }
                        //check if the key exists and value doesn't,trim any trailing spaces
                        if (user_follower.ContainsKey(item.Trim()))
                        {
                            foreach (var userFollower in user_follower)
                            {
                                if (!userFollower.Value.Contains(follower))
                                {
                                    user_follower[item.Trim()].Add(follower);
                                }
                            }

                        }
                    }
                }
                else
                {
                    List<string> followerList = new List<string>();
                    followerList.Add(follower);
                    if (!user_follower.ContainsKey(user))
                    {

                        user_follower.Add(user, followerList);

                    }
                    // if key already exist but the value doesnt update the dictionary with new value
                    if (user_follower.ContainsKey(user) && !user_follower.ContainsValue(followerList))
                    {
                        user_follower[user].Add(follower);

                    }

                }
            }
            return user_follower;
        }

        public static string[] ReadFile(string fileName)
        {
            List<string> lines = new List<string>();
            try
            {
                string path = Environment.CurrentDirectory + @"\Files\" + fileName;
                foreach (var line in File.ReadAllLines(path))
                {
                    lines.Add(line);
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine("The process failed: {0}", ex.ToString());
            }
            return lines.ToArray();
        }

        public static void DiplayOutput(Dictionary<string, List<string>> tweetsDictionary, Dictionary<string, List<string>> userDictionary,List<string> userList)
        {
            var usersValue = "";
            var usersKey = "";
            var tweetMessage = "";
            foreach (var user in userList)
            {
                Console.WriteLine(user);
                Console.WriteLine();
                
                foreach (var tweet in tweetsDictionary)
                {
                    foreach (var tweetItem in tweet.Value)
                    {
                        tweetMessage = tweetItem;
                        foreach (var userKey in userDictionary)
                        {
                            usersKey = userKey.Key;
                            foreach (var userValue in userKey.Value)
                            {
                                usersValue = userValue;
                            }
                        }
                        if ((user == tweet.Key || user == usersKey || user == usersValue) && (user == tweet.Key || user == usersValue))
                        {
                            //if  tweet message is bigger than 140, use 140 else use the full length.
                            var tweets = tweetMessage.Substring(0, tweetMessage.Length > 140 ? 140 : tweetMessage.Length);
                            Console.WriteLine("\t@" + tweet.Key + ":" +" "+ tweets);

                        }
                    }
                    
                }

            }
            Console.ReadLine();
        }

    }
}

