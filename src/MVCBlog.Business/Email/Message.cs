namespace MVCBlog.Business.Email;

public class Message
{
    private readonly List<Recipient> bccRecipients = new List<Recipient>();

    private readonly List<Attachment> attachments = new List<Attachment>();

    private readonly List<EmbeddedImage> embeddedImages = new List<EmbeddedImage>();

    public Message(Recipient bccRecipient, string subject, string bodyAsHtml)
    {
        this.bccRecipients.Add(bccRecipient ?? throw new ArgumentNullException(nameof(bccRecipient)));
        this.Subject = subject ?? throw new ArgumentNullException(nameof(subject));
        this.BodyAsHtml = bodyAsHtml ?? throw new ArgumentNullException(nameof(bodyAsHtml));
    }

    public Message(IEnumerable<Recipient> bccRecipients, string subject, string bodyAsHtml)
    {
        this.bccRecipients.AddRange(bccRecipients ?? throw new ArgumentNullException(nameof(bccRecipients)));
        this.Subject = subject ?? throw new ArgumentNullException(nameof(subject));
        this.BodyAsHtml = bodyAsHtml ?? throw new ArgumentNullException(nameof(bodyAsHtml));
    }

    public Recipient? Sender { get; private set; }

    public Recipient? ReplyTo { get; private set; }

    public IReadOnlyCollection<Recipient> BccRecipients
    {
        get
        {
            return this.bccRecipients;
        }
    }

    public string Subject { get; }

    public string BodyAsHtml { get; }

    public IReadOnlyCollection<Attachment> Attachments
    {
        get
        {
            return this.attachments;
        }
    }

    public IReadOnlyCollection<EmbeddedImage> EmbeddedImages
    {
        get
        {
            return this.embeddedImages;
        }
    }

    public Message WithSender(Recipient sender)
    {
        this.Sender = sender ?? throw new ArgumentNullException(nameof(sender));

        return this;
    }

    public Message WithReplyTo(Recipient replyTo)
    {
        this.ReplyTo = replyTo ?? throw new ArgumentNullException(nameof(replyTo));

        return this;
    }

    public Message AddRecipient(Recipient recipient)
    {
        this.bccRecipients.Add(recipient ?? throw new ArgumentNullException(nameof(recipient)));

        return this;
    }

    public Message AddAttachment(Attachment attachment)
    {
        this.attachments.Add(attachment ?? throw new ArgumentNullException(nameof(attachment)));

        return this;
    }

    public Message AddEmbeddedImage(EmbeddedImage embeddedImage)
    {
        this.embeddedImages.Add(embeddedImage ?? throw new ArgumentNullException(nameof(embeddedImage)));

        return this;
    }
}