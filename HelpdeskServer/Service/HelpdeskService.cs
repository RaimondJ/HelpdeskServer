using HelpdeskServer.Models;

namespace HelpdeskServer.Service
{
    public class HelpdeskService
    {
        private PostRepository repository;
        public HelpdeskService(PostRepository postRepository) 
        {
            repository = postRepository;
        }

        public async void addPost(Post post)
        {
            await repository.AddAsync(post);
        }


    }
}
